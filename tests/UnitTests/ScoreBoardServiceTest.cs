using Eol.Cpl.Client;
using System.Linq;
using Xunit;

namespace Eol.Cpl.UnitTest
{
    public class ScoreBoardServiceTest
    {
        private CurrentMatchDetails _currentMatch;
        private PlayerDetails striker;
        private PlayerDetails nonStriker;
        private double currentOver = 0;

        public ScoreBoardServiceTest()
        {
            MatchInitialize initialize = new MatchInitialize();
            _currentMatch = initialize.GetCurrentMatchDetails();
            striker = _currentMatch.Players.First(t => t.IsOnField);
            nonStriker = _currentMatch.Players.Last(t => t.IsOnField);
            _currentMatch.BallsRemaining = _currentMatch.OversLeft * 6;
        }

        [Fact]
        public void UpdateScore_ScoreUpdated_Verify()
        {
            int runScored = 5;
            var expectedBallsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin - runScored;

            ScoreBoardService.UpdateScore(_currentMatch, striker, runScored, currentOver);           

            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.Equal(1, striker.BallsFaced);
            Assert.Equal(5, striker.RunsScored);
        }

        [Fact]
        public void UpdateWicket_ScoreUpdated_Verify()
        {      
            var expectedBallsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin;
            var wicketsLeft = _currentMatch.WicketsLeft - 1;
            var currentStriker = striker;

            ScoreBoardService.UpdateWicket(_currentMatch, ref striker, nonStriker, currentOver);

            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.NotEqual(currentStriker, striker);
            Assert.Equal(wicketsLeft, _currentMatch.WicketsLeft);
            Assert.True(currentStriker.IsOut);
            Assert.False(currentStriker.IsOnField);
        }
    }
}
