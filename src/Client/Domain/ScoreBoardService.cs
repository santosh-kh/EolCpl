using System;
using System.Linq;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Class contains methods to ppdates the score card along with commentary.
    /// </summary>
    public static class ScoreBoardService
    {
        /// <summary>
        /// Updates the score card as well as striker score based on the runScored.
        /// </summary>
        /// <param name="currentMatch"></param>
        /// <param name="striker"></param>
        /// <param name="runsScored"></param>
        /// <param name="currentOver"></param>
        public static void UpdateScore(CurrentMatchDetails currentMatch, PlayerDetails striker, int runsScored, double currentOver)
        {
            currentMatch.BallsRemaining -= 1;
            striker.RunsScored += runsScored;
            striker.BallsFaced += 1;
            currentMatch.RunsNeedToWin -= runsScored;

            UpdateCommentory(striker, currentOver, runsScored);
            if (currentMatch.RunsNeedToWin <= 0)
            {
                DisplayMatchResult(currentMatch);
            }
        }

        /// <summary>
        /// Updates commentary for each ball based on currentOver, runsScored or isOut
        /// </summary>
        /// <param name="striker"></param>
        /// <param name="currentOver"></param>
        /// <param name="runsScored"></param>
        /// <param name="isOut"></param>
        public static void UpdateCommentory(PlayerDetails striker, double currentOver, int runsScored, bool isOut = false)
        {
            Console.WriteLine($"{currentOver} {striker.Name} {(isOut ? "is out" : runsScored == 0 ? "no run" : "scores")} {(runsScored > 0 ? runsScored.ToString() : "")}");
        }

        /// <summary>
        /// Updates the score card as well as striker for an wicket.
        /// </summary>
        /// <remarks>Gets the new batsman if wickets left else ends the match by printing match summary.</remarks>
        /// <param name="currentMatch"></param>
        /// <param name="striker"></param>
        /// <param name="nonStriker"></param>
        /// <param name="currentOver"></param>
        public static void UpdateWicket(CurrentMatchDetails currentMatch, ref PlayerDetails striker, PlayerDetails nonStriker, double currentOver)
        {
            striker.BallsFaced += 1;
            currentMatch.BallsRemaining -= 1;
            striker.IsOut = true;
            striker.IsOnField = false;
            currentMatch.WicketsLeft -= 1;

            UpdateCommentory(striker, currentOver, 0, true);
            if (currentMatch.WicketsLeft == 0)
            {
                DisplayMatchResult(currentMatch, false);
            }
            else
            {
                int battingOrder = striker.BattingOrder;
                striker = striker.BattingOrder < nonStriker.BattingOrder ?
                    currentMatch.Players.FirstOrDefault(t => t.BattingOrder.Equals(nonStriker.BattingOrder + 1)) :
                    currentMatch.Players.FirstOrDefault(t => t.BattingOrder.Equals(battingOrder + 1));
                striker.IsOnField = true;
            }
        }

        /// <summary>
        /// Prints the match result along with batsman scre details.
        /// </summary>
        /// <param name="currentMatch"></param>
        /// <param name="isWonByChasing"></param>
        public static void DisplayMatchResult(CurrentMatchDetails currentMatch, bool isWonByChasing = true)
        {
            if (isWonByChasing)
            {
                Console.WriteLine($"{Environment.NewLine}{currentMatch.Team2_Name} won by {currentMatch.WicketsLeft} wickets with {currentMatch.BallsRemaining} balls remaining.");
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}{currentMatch.Team1_Name} won the match with {currentMatch.BallsRemaining} balls remaining.");
            }
            foreach (var player in currentMatch.Players)
            {
                Console.WriteLine($"{player.Name} - {player.RunsScored}{(!player.IsOut && player.IsOnField ? "*" : "")} runs ({player.BallsFaced})");
            }
            Console.ReadKey();
            System.Environment.Exit(1);
        }
    }
}
