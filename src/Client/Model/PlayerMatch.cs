
namespace Eol.Cpl.Client
{
    /// <summary>
    /// Conatins properties to hold information about player match details.
    /// </summary>
    public class PlayerMatch
    {
        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        public int RunsScored { get; set; }
        public int BallsFaced { get; set; }
        public int BattingOrder { get; set; }
        public bool IsOut { get; set; }
        public bool IsOnField { get; set; }
    }
}
