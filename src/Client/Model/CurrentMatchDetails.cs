using System.Collections.Generic;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Score card details along with players information.
    /// </summary>
    public class CurrentMatchDetails
    {
        public string Team1_Name { get; set; }
        public string Team2_Name { get; set; }
        public int OversLeft { get; set; }
        public int? RunsNeedToWin { get; set; }
        public int WicketsLeft { get; set; }
        public List<PlayerDetails> Players { get; set; }
        public int BallsRemaining { get; set; }
    }
}