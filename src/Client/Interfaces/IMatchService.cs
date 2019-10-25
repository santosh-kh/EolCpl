
namespace Eol.Cpl.Client
{
    public interface IMatchService
    {
        void SimulateMatch(CurrentMatchDetails currentMatch);
        void NextOver(CurrentMatchDetails currentMatch, ref PlayerDetails striker, ref PlayerDetails nonStriker, ref double currentOver);
        void NextBall(CurrentMatchDetails currentMatch, ref PlayerDetails striker, ref PlayerDetails nonStriker, double currentOver);
    }
}
