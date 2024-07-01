using System.Threading.Tasks;

namespace IW4MAdmin.Plugins.Stats.Client.Abstractions
{
    public interface IServerDistributionCalculator
    {
        Task Initialize();
        Task<double> GetZScoreForServer(long serverId, double value);
        Task<double?> GetRatingForZScore(double? value);
    }
}