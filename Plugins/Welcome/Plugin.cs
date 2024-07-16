using System.Threading.Tasks;
using SharedLibraryCore;
using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Database.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using Humanizer;
using Data.Abstractions;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore.Events.Management;
using SharedLibraryCore.Interfaces.Events;
using static Data.Models.Client.EFClient;

namespace IW4MAdmin.Plugins.Welcome;

public class Plugin : IPluginV2
{
    public string Author => "RaidMax";
    public string Version => Utilities.GetVersionAsString();
    public string Name => "Welcome Plugin";

    private readonly WelcomeConfiguration _configuration;
    private readonly IDatabaseContextFactory _contextFactory;

    public Plugin(WelcomeConfiguration configuration, IDatabaseContextFactory contextFactory)
    {
        _configuration = configuration;
        _contextFactory = contextFactory;

        IManagementEventSubscriptions.ClientStateAuthorized += OnClientStateAuthorized;
    }

    public static void RegisterDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddConfiguration("WelcomePluginSettings", new WelcomeConfiguration());
    }

    private async Task OnClientStateAuthorized(ClientStateEvent clientState, CancellationToken token)
    {
        var player = clientState.Client;

        var isPrivileged = player.Level >= Permission.Trusted && !player.Masked ||
                           !string.IsNullOrEmpty(player.Tag) && player.Level != Permission.Flagged &&
                           player.Level != Permission.Banned && !player.Masked;

        if (isPrivileged)
        {
            var announcement = await ProcessAnnouncement(_configuration.PrivilegedAnnouncementMessage, player);
            player.CurrentServer.Broadcast(announcement);
        }

        // Send welcome message to the player
        player.Tell(await ProcessAnnouncement(_configuration.UserWelcomeMessage, player));

        if (player.Level == Permission.Flagged)
        {
            var penaltyReason = await GetPenaltyReason(player.ClientId, token);

            player.CurrentServer.ToAdmins(
                Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_FLAG_MESSAGE"]
                    .FormatExt(player.Name, penaltyReason)
            );
        }
        else
        {
            var announcement = await ProcessAnnouncement(_configuration.UserAnnouncementMessage, player);
            player.CurrentServer.Broadcast(announcement);
        }
    }

    private async Task<string> GetPenaltyReason(int clientId, CancellationToken token)
    {
        await using var context = _contextFactory.CreateContext(false);

        return await context.Penalties
            .Where(p => p.OffenderId == clientId && p.Type == EFPenalty.PenaltyType.Flag)
            .OrderByDescending(p => p.When)
            .Select(p => p.AutomatedOffense ?? p.Offense)
            .FirstOrDefaultAsync(token) ?? string.Empty;
    }

    private static async Task<string> ProcessAnnouncement(string messageTemplate, EFClient player)
    {
        var levelWithColor = $"{Utilities.ConvertLevelToColor(player.Level, player.ClientPermission.Name)}{(string.IsNullOrEmpty(player.Tag)
            ? string.Empty
            : $" (Color::White){player.Tag}(Color::White)")}";

        return messageTemplate
            .Replace("{{ClientName}}", player.Name)
            .Replace("{{ClientLevel}}", levelWithColor)
            .Replace("{{ClientLocation}}", await GetCountryName(player.IPAddressString))
            .Replace("{{TimesConnected}}", player.Connections.Ordinalize(Utilities.CurrentLocalization.Culture));
    }

    private static async Task<string> GetCountryName(string ipAddress)
    {
        try
        {
            using var httpClient = new HttpClient();
            var apiUrl =
                $"http://ip-api.com/json/{ipAddress}?lang={Utilities.CurrentLocalization.LocalizationName.Split('-').First().ToLower()}";

            var response = await httpClient.GetFromJsonAsync<IpApiResponse>(apiUrl);

            return response?.Country ?? Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_UNKNOWN_COUNTRY"];
        }
        catch
        {
            return Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_UNKNOWN_IP"];
        }
    }

    private record IpApiResponse(string Country);
}
