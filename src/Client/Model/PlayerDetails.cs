using System;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Conatins properties to hold information about player match details.
    /// </summary>
    public class PlayerDetails : Player
    {
        static Random random = new Random();

        public int MatchId { get; set; }
        public int RunsScored { get; set; }
        public int BallsFaced { get; set; }
        public int BattingOrder { get; set; }
        public bool IsOut { get; set; }
        public bool IsOnField { get; set; }

        /// <summary>
        /// Returns run scored based on the player score probailities.
        /// </summary>
        /// <returns>Runs scored</returns>
        public int Strike()
        {
            var r = random.Next(0, 101);
            int score = 0;
            for (int i = 0; i < ScoreProbabilities.Length; i++)
            {
                if (r <= ScoreProbabilities[i])
                {
                    score = i;
                    break;
                }
            }
            return score;
        }
    }
}