
namespace Eol.Cpl.Client
{
    public interface IPlayerScoreHelper
    {
        void BuildCumulativeDistribution(PlayerDetails player);
        int Strike(PlayerDetails player);
    }
}
