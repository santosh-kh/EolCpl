using System;
using System.Linq;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// The class contains methods for the match simulation.
    /// </summary>
    public class MatchService : IMatchService
    {
        private readonly IPlayerScoreHelper _playerScoreHelper;

        public MatchService(IPlayerScoreHelper playerScoreHelper)
        {
            _playerScoreHelper = playerScoreHelper;
        }
        /// <summary>
        /// Simulates the match for the remaining overs.
        /// </summary>
        /// <remarks>Prints the match result once it's done.</remarks>
        /// <param name="currentMatch">currentMatch details</param>
        public void SimulateMatch(CurrentMatchDetails currentMatch)
        {
            PlayerDetails striker = currentMatch.Players.First(t => t.IsOnField);
            PlayerDetails nonStriker = currentMatch.Players.Last(t => t.IsOnField);
            var oversLeft = currentMatch.OversLeft;
            currentMatch.BallsRemaining = oversLeft * 6;
            double currentOver = 0;
            for (int i = 1; i <= oversLeft; i++)
            {
                Console.WriteLine($"{Environment.NewLine}{currentMatch.OversLeft} overs left. {currentMatch.RunsNeedToWin} runs to win.");
                NextOver(currentMatch, ref striker, ref nonStriker, ref currentOver);
                currentMatch.OversLeft -= 1;
                currentOver += 0.4;
            }
            ScoreBoardService.DisplayMatchResult(currentMatch, false);
        }


        /// <summary>
        /// Simulates the over of the match. 
        /// </summary>
        /// <remarks>Rotate the striker at the end of over.</remarks>
        /// <param name="currentMatch"></param>
        /// <param name="striker"></param>
        /// <param name="nonStriker"></param>
        /// <param name="currentOver"></param>
        public void NextOver(CurrentMatchDetails currentMatch, ref PlayerDetails striker, ref PlayerDetails nonStriker, ref double currentOver)
        {
            for (int j = 1; j <= 6; j++)
            {
                currentOver += 0.1;
                NextBall(currentMatch, ref striker, ref nonStriker, currentOver);
            }
            RotateStriker(ref striker, ref nonStriker);
        }

        /// <summary>
        /// Simulates the ball of the match.
        /// </summary>
        /// <remarks>Updates the score card based on the player score.</remarks>
        /// <param name="currentMatch"></param>
        /// <param name="striker"></param>
        /// <param name="nonStriker"></param>
        /// <param name="currentOver"></param>
        public void NextBall(CurrentMatchDetails currentMatch, ref PlayerDetails striker, ref PlayerDetails nonStriker, double currentOver)
        {
            switch (_playerScoreHelper.Strike(striker))
            {
                case 0:
                    currentMatch.BallsRemaining -= 1;
                    striker.BallsFaced += 1;
                    ScoreBoardService.UpdateCommentory(striker, currentOver, 0);
                    break;
                case 1:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 1, currentOver);
                    RotateStriker(ref striker, ref nonStriker);
                    break;
                case 2:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 2, currentOver);
                    break;
                case 3:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 3, currentOver);
                    RotateStriker(ref striker, ref nonStriker);
                    break;
                case 4:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 4, currentOver);
                    break;
                case 5:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 5, currentOver);
                    RotateStriker(ref striker, ref nonStriker);
                    break;
                case 6:
                    ScoreBoardService.UpdateScore(currentMatch, striker, 6, currentOver);
                    break;
                case 7:
                    ScoreBoardService.UpdateWicket(currentMatch, ref striker, nonStriker, currentOver);
                    break;
                default: throw new ArgumentException("Ivalid strike");
            }
        }

        private static void RotateStriker(ref PlayerDetails striker, ref PlayerDetails nonStriker)
        {
            var temp = striker;
            striker = nonStriker;
            nonStriker = temp;
        }
    }
}
