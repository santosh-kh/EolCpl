using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Eol.Cpl.Client
{
    public class PlayerScoreHelper : IPlayerScoreHelper
    {
        public void BuildCumulativeDistribution(PlayerDetails player)
        {
            int sum = 0;

            for (int i = 0; i < player.ScoreProbabilities.Length; i++)
            {
                sum += player.ScoreProbabilities[i];
            }
            if (sum != 100)
            {
                Console.WriteLine($"{Environment.NewLine}Probabilities doesn't add upto 100 for player {player.Name}");
                throw new Exception("Probabilities doesn't add upto 100");                
            }
            for (int i = 0; i < player.ScoreProbabilities.Length; i++)
            {
                if (i != 0)
                {
                    player.ScoreProbabilities[i] += player.ScoreProbabilities[i - 1];
                }
            }
        }

        //Wrapper for unit test
        public int Strike(PlayerDetails player)
        {
            return player.Strike();
        }
    }
}
