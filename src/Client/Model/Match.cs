
namespace Eol.Cpl.Client
{
    /// <summary>
    /// Conatins properties to hold information about match.
    /// </summary>
    public class Match
    {
        public int Id { get; set; }
        public int Team1_Id { get; set; }
        public int Team2_Id { get; set; }
        public int BatFirstTeamId { get; set; }
        public int? Team1_Score { get; set; }
        public int? Team2_Score { get; set; }
        public int Team1_OversPlayed { get; set; }
        public int Team2_OversPlayed { get; set; }
        public int Team1_WicketsLeft { get; set; }
        public int Team2_WicketsLeft { get; set; }
        public int Innings { get; set; }
        public bool IsOver { get; set; }
    }
}
