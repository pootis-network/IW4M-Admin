using System.Collections.Generic;
using System.Linq;
using Data.Models.Client;
using Data.Models.Client.Stats;
using Microsoft.EntityFrameworkCore;
using SharedLibraryCore.Interfaces;

namespace IW4MAdmin.Application.Extensions;

public static class ScriptPluginExtensions
{
    public static IEnumerable<object> GetClientsBasicData(
        this DbSet<Data.Models.Client.EFClient> set, int[] clientIds)
    {
        return set.Where(client => clientIds.Contains(client.ClientId))
            .Select(client => new
            {
                client.ClientId,
                client.CurrentAlias,
                client.Level,
                client.NetworkId
            }).ToList();
    }

    public static IEnumerable<object> GetClientsStatData(this DbSet<EFClientStatistics> set, int[] clientIds,
        double serverId)
    {
        return set.Where(stat => clientIds.Contains(stat.ClientId) && stat.ServerId == (long)serverId).ToList();
    }

    public static EFClient GetClientByNumber(this IGameServer server, int clientNumber) =>
        server.ConnectedClients.FirstOrDefault(client => client.ClientNumber == clientNumber);

    public static EFClient GetClientByGuid(this IGameServer server, string clientGuid) =>
        server.ConnectedClients.FirstOrDefault(client => client?.GuidString == clientGuid?.Trim().ToLower());

    public static EFClient GetClientByXuid(this IGameServer server, string clientGuid) =>
        server.ConnectedClients.FirstOrDefault(client => client?.XuidString == clientGuid?.Trim().ToLower());
    
    public static EFClient GetClientByDecimalGuid(this IGameServer server, string clientGuid) =>
        server.ConnectedClients.FirstOrDefault(client => client.NetworkId.ToString() == clientGuid?.Trim().ToLower());
}
