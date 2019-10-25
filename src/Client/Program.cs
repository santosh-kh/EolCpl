using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// App starts fromhere.
    /// </summary>
    public static class Program
    {
        private static MatchInitialize _initializeMatchData = new MatchInitialize();

        /// <summary>
        /// Reads match data and handles match simulation.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Startup startup = new Startup();

            startup.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var playerScoreHelper = serviceProvider.GetService<IPlayerScoreHelper>();
            var matchService = serviceProvider.GetService<IMatchService>();

            Console.WriteLine("Welcome to EOL Cricket Premier League!");

            var currentMatch = _initializeMatchData.GetCurrentMatchDetails();

            Console.WriteLine($"{Environment.NewLine}Score board : {currentMatch.Team2_Name} needs {currentMatch.RunsNeedToWin} runs in {currentMatch.OversLeft * 6} balls, " +
                $"{currentMatch.WicketsLeft} wickets in hand.");

            try
            {
                foreach (var player in currentMatch.Players)
                {
                    playerScoreHelper.BuildCumulativeDistribution(player);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(1500);
                System.Environment.Exit(1);
            }
            matchService.SimulateMatch(currentMatch);            
        }        
    }
}
