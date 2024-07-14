using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore;
using SharedLibraryCore.Database.Models;
using SharedLibraryCore.Events.Game;
using SharedLibraryCore.Events.Management;
using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Interfaces.Events;

namespace IW4MAdmin.Plugins.ProfanityDeterment;

public class Plugin : IPluginV2
{
    public string Name => "ProfanityDeterment";
    public string Version => Utilities.GetVersionAsString();
    public string Author => "RaidMax";

    private const string ProfanityKey = "_profanityInfringements";

    private readonly ProfanityDetermentConfiguration _configuration;

    public Plugin(ProfanityDetermentConfiguration configuration)
    {
        _configuration = configuration;

        if (!_configuration.EnableProfanityDeterment)
        {
            return;
        }

        IManagementEventSubscriptions.ClientStateInitialized += OnClientStateInitialized;
        IGameEventSubscriptions.ClientMessaged += GameEventSubscriptionsOnClientMessaged;
        IManagementEventSubscriptions.ClientStateDisposed += (clientEvent, _) =>
        {
            clientEvent.Client.SetAdditionalProperty(ProfanityKey, null);
            return Task.CompletedTask;
        };
    }

    public static void RegisterDependencies(IServiceCollection serviceProvider)
    {
        serviceProvider.AddConfiguration<ProfanityDetermentConfiguration>("ProfanityDetermentSettings");
    }

    private Task GameEventSubscriptionsOnClientMessaged(ClientMessageEvent clientEvent, CancellationToken token)
    {
        if (!_configuration.EnableProfanityDeterment)
        {
            return Task.CompletedTask;
        }

        var sender = clientEvent.Server.AsConsoleClient();
        var messages = FindOffensiveWordsAndWarn(sender, clientEvent.Message);
        if (messages.Count is 0)
        {
            return Task.CompletedTask;
        }

        var profanityInfringements = clientEvent.Origin.GetAdditionalProperty<int>(ProfanityKey);
        if (profanityInfringements >= _configuration.KickAfterInfringementCount)
        {
            clientEvent.Client.Kick(_configuration.ProfanityKickMessage, sender);
        }
        else if (profanityInfringements < _configuration.KickAfterInfringementCount)
        {
            clientEvent.Client.SetAdditionalProperty(ProfanityKey, profanityInfringements + 1);
            clientEvent.Client.Warn(_configuration.ProfanityWarningMessage, sender);
        }

        return Task.CompletedTask;
    }

    private Task OnClientStateInitialized(ClientStateInitializeEvent clientEvent, CancellationToken token)
    {
        if (!_configuration.EnableProfanityDeterment)
        {
            return Task.CompletedTask;
        }

        if (!_configuration.KickOnInfringingName)
        {
            return Task.CompletedTask;
        }

        clientEvent.Client.SetAdditionalProperty(ProfanityKey, 0);

        var sender = clientEvent.Client.CurrentServer.AsConsoleClient();
        var messages = FindOffensiveWordsAndWarn(sender, clientEvent.Client.CleanedName);
        if (messages.Count is 0)
        {
            return Task.CompletedTask;
        }

        clientEvent.Client.Kick(_configuration.ProfanityKickMessage, sender);
        return Task.CompletedTask;
    }

    private List<string> FindOffensiveWordsAndWarn(EFClient sender, string message)
    {
        var offensiveWords = _configuration.OffensiveWords;
        var matchedWords = offensiveWords.Where(word => Regex.IsMatch(message.StripColors(), word, RegexOptions.IgnoreCase)).ToList();

        sender.AdministeredPenalties =
        [
            new EFPenalty
            {
                AutomatedOffense = $"{message} - {string.Join(",", matchedWords)}"
            }
        ];

        return matchedWords;
    }
}
