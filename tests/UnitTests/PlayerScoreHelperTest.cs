using Eol.Cpl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Eol.Cpl.UnitTest
{
    /// <summary>
    /// Tests to verify players scores against their probabilities.
    /// </summary>
    public class PlayerScoreHelperTest
    {
        /// <summary>
        /// Test to verify player probabilities add upto 100 or not
        /// </summary>
        /// <exception cref="System.Exception">Thrown when player probabilities don't add upto 100.</exception>
        [Fact]
        public void PlayerWithProbabilities_NotEqualHundred_Exception()
        {
            PlayerDetails player = new PlayerDetails
            {
                Name = "Pravin",
                ScoreProbabilities = new int[8] { 5, 30, 25, 10, 15, 1, 9, 6 }
            };

            PlayerScoreHelper playerScoreHelper = new PlayerScoreHelper();
            Assert.Throws<Exception>(() => playerScoreHelper.BuildCumulativeDistribution(player));
        }

        /// <summary>
        /// Test to verify player won't score for which he has zero probability
        /// </summary>
        [Fact]
        public void PlayerPobability_Zero_NerverScores()
        {
            //Score 2 has zero probability
            PlayerDetails player = new PlayerDetails
            {
                Name = "Pravin",
                ScoreProbabilities = new int[8] { 5, 30, 0, 10, 15, 1, 9, 5 }
            };

            PlayerScoreHelper playerScoreHelper = new PlayerScoreHelper();

            List<int> playerScores = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                playerScores.Add(player.Strike());
            }
            Assert.Empty(playerScores.Where(t => t == 2).ToList());
        }

        /// <summary>
        /// Test to verify player scores against their probabilities.
        /// </summary>
        [Fact]
        public void InidvidualPlayerStrikeProbalility_Verify()
        {
            PlayerDetails player = new PlayerDetails
            {
                Name = "Pravin",
                ScoreProbabilities = new int[8] { 5, 30, 25, 10, 15, 1, 9, 5 }
            };

            PlayerScoreHelper playerScoreHelper = new PlayerScoreHelper();
            playerScoreHelper.BuildCumulativeDistribution(player);

            double scoreZero = 0, scoreOne = 0, scoreTwo = 0, scoreThree = 0, scoreFour = 0, scoreFive = 0, scoreSix = 0, scoreOut = 0;

            List<int> playerScores = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                playerScores.Add(player.Strike());
            }

            scoreZero = ((playerScores.Where(t => t == 0).Count()) / 100);
            scoreOne = ((playerScores.Where(t => t == 1).Count()) / 100);
            scoreTwo = ((playerScores.Where(t => t == 2).Count()) / 100);
            scoreThree = ((playerScores.Where(t => t == 3).Count()) / 100);
            scoreFour = ((playerScores.Where(t => t == 4).Count()) / 100);
            scoreFive = ((playerScores.Where(t => t == 5).Count()) / 100);
            scoreSix = ((playerScores.Where(t => t == 6).Count()) / 100);
            scoreOut = ((playerScores.Where(t => t == 7).Count()) / 100);

            //probability +/- 1
            Assert.Contains(scoreZero, new double[] { 5, 4, 6 });
            Assert.Contains(scoreOne, new double[] { 29, 30, 31 });
            Assert.Contains(scoreTwo, new double[] { 24, 25, 26 });
            Assert.Contains(scoreThree, new double[] { 9, 10, 11 });
            Assert.Contains(scoreFour, new double[] { 14, 15, 16 });
            Assert.Contains(scoreFive, new double[] { 0, 1, 2 });
            Assert.Contains(scoreSix, new double[] { 8, 9, 10 });
            Assert.Contains(scoreOut, new double[] { 4, 5, 6 });            
        }

        /// <summary>
        /// Test to verify players scores against their probabilities.
        /// </summary>
        [Fact]
        public void MultiPlayersStrikeProbalility_Verify()
        {
            MatchInitialize initialize = new MatchInitialize();
            var currentMatch = initialize.GetCurrentMatchDetails();

            PlayerScoreHelper playerScoreHelper = new PlayerScoreHelper();

            foreach (var player in currentMatch.Players)
            {
                playerScoreHelper.BuildCumulativeDistribution(player);
            }
            Dictionary<string, List<int>> playerScores = new Dictionary<string, List<int>>();

            for (int i = 0; i < 10000; i++)
            {
                foreach (var player in currentMatch.Players)
                {
                    var score = player.Strike();
                    if (playerScores.ContainsKey(player.Name))
                    {
                        var scores = playerScores[player.Name];
                        scores.Add(score);
                        playerScores[player.Name] = scores;
                    }
                    else
                    {
                        playerScores.Add(player.Name, new List<int> { score });
                    }
                }
            }

            Assert.Contains(playerScores["Pravin"].Where(t => t == 0).Count() / 100, new double[] { 5, 4, 6 });
            Assert.Contains(playerScores["Irfan"].Where(t => t == 0).Count() / 100, new double[] { 9, 10, 11 });
            Assert.Contains(playerScores["Jalindar"].Where(t => t == 0).Count() / 100, new double[] { 19, 20, 21 });
            Assert.Contains(playerScores["Vaishali"].Where(t => t == 0).Count() / 100, new double[] { 29, 30, 31 });

        }
    }
}
