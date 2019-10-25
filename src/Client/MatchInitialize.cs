using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Initializes the current match data.
    /// It reads data from matchData.json file.
    /// </summary>
    public class MatchInitialize
    {
        private const int _overs = 20;
        private readonly EolCplData _data;
        public MatchInitialize()
        {
            var dataFile = Path.Combine(Directory.GetCurrentDirectory(), "matchData.json");
            _data = JsonConvert.DeserializeObject<EolCplData>(File.ReadAllText(dataFile));
        }

        /// <summary>
        /// Reads the current match details along with player details.
        /// </summary>
        /// <returns>Eol.Cpl.Client.CurrentMatchDetails</returns>
        public CurrentMatchDetails GetCurrentMatchDetails()
        {
            var match = _data.Matches.FirstOrDefault(t => !t.IsOver);
            var players = (from p in _data.Players
                           join mp in _data.PlayerMatches
                           on p.Id equals mp.PlayerId
                           where p.TeamId.Equals(match.Team2_Id) && mp.MatchId.Equals(match.Id) && mp.IsOut.Equals(false)
                           select new PlayerDetails
                           {
                               Id = p.Id,
                               Name = p.Name,
                               MatchId = mp.MatchId,
                               ScoreProbabilities = p.ScoreProbabilities,
                               BallsFaced = mp.BallsFaced,
                               BattingOrder = mp.BattingOrder,
                               RunsScored = mp.RunsScored,
                               IsOut = mp.IsOut,
                               IsOnField = mp.IsOnField,
                               TeamId = p.TeamId
                           }).OrderBy(t => t.BattingOrder).ToList();

            return new CurrentMatchDetails
            {
                Team1_Name = _data.Teams.FirstOrDefault(t => t.Id.Equals(match.Team1_Id)).Name,
                Team2_Name = _data.Teams.FirstOrDefault(t => t.Id.Equals(match.Team2_Id)).Name,
                OversLeft = _overs - match.Team2_OversPlayed,
                RunsNeedToWin = match.Team1_Score - match.Team2_Score + 1,
                WicketsLeft = match.Team2_WicketsLeft,
                Players = players
            };
        }
    }
}
