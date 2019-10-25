using Eol.Cpl.Client;
using Moq;
using System.Linq;
using Xunit;

namespace Eol.Cpl.UnitTest
{
    /// <summary>
    /// Tests verify the score card after each ball, with different strike.
    /// </summary>
    public class MatchServiceTest
    {
        private readonly Mock<IPlayerScoreHelper> _playerScoreHelper;
        private CurrentMatchDetails _currentMatch;
        private PlayerDetails striker;
        private PlayerDetails nonStriker;
        private double currentOver = 0;
        private MatchService target;

        public MatchServiceTest()
        {
            _playerScoreHelper = new Mock<IPlayerScoreHelper>();
            MatchInitialize initialize = new MatchInitialize();
            _currentMatch = initialize.GetCurrentMatchDetails();
            striker = _currentMatch.Players.First(t => t.IsOnField);
            nonStriker = _currentMatch.Players.Last(t => t.IsOnField);
            _currentMatch.BallsRemaining = _currentMatch.OversLeft * 6;

            target = new MatchService(_playerScoreHelper.Object);
        }

        /// <summary>
        /// Test to verify NextOver() method from MatchService
        /// </summary>
        [Fact]
        public void SimulateOver_CheckScoreCard_Verify()
        {
            _playerScoreHelper.Setup(t => t.Strike(striker)).Returns(2);

            var expectedBallsRemaining = _currentMatch.BallsRemaining - 6;
            var runsNeedToWin = _currentMatch.RunsNeedToWin - 12;
            var currentStriker = striker;

            target.NextOver(_currentMatch, ref striker, ref nonStriker, ref currentOver);

            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            //Striker rotated after an over
            Assert.NotSame(currentStriker, striker);
        }


        /// <summary>
        /// Test to verify NextBall() for Wicket scenario
        /// </summary>
        [Fact]
        public void SimulateBall_Out_Verify()
        {
            _playerScoreHelper.Setup(t => t.Strike(striker)).Returns(7);

            var ballsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin;
            var currentStriker = striker;
            var nextBatsman = _currentMatch.Players.FirstOrDefault(t => t.Name.Equals("Jalindar"));

            target.NextBall(_currentMatch, ref striker, ref nonStriker, currentOver);

            Assert.True(currentStriker.IsOut);
            Assert.False(currentStriker.IsOnField);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.Equal(ballsRemaining, _currentMatch.BallsRemaining);
            //Striker change after wicket
            Assert.NotSame(currentStriker, striker);
            Assert.Same(nextBatsman, striker);
        }

        /// <summary>
        /// Test to verify NextBall() for odd score scenario
        /// </summary>
        [Fact]
        public void SimulateBall_OddScore_RotateStrike()
        {
            _playerScoreHelper.Setup(t => t.Strike(striker)).Returns(3);

            var expectedBallsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin - 3;
            var currentStriker = striker;

            target.NextBall(_currentMatch, ref striker, ref nonStriker, currentOver);

            Assert.False(currentStriker.IsOut);
            Assert.True(currentStriker.IsOnField);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            //Striker change after wicket
            Assert.NotSame(currentStriker, striker);
            Assert.Same(currentStriker, nonStriker);
        }

        /// <summary>
        /// Test to verify NextBall() for even score scenario
        /// </summary>
        [Fact]
        public void SimulateBall_EvenScore_NoRotateStrike()
        {    
            _playerScoreHelper.Setup(t => t.Strike(striker)).Returns(4);

            var expectedBallsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin - 4;
            var currentStriker = striker;

            target.NextBall(_currentMatch, ref striker, ref nonStriker, currentOver);

            Assert.False(currentStriker.IsOut);
            Assert.True(currentStriker.IsOnField);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            //No change in striker
            Assert.Same(currentStriker, striker);
        }

        /// <summary>
        /// Test to verify NextBall() for no run scenario
        /// </summary>
        [Fact]
        public void SimulateBall_NoRun_NoRotateStrike()
        {
            _playerScoreHelper.Setup(t => t.Strike(striker)).Returns(0);

            var expectedBallsRemaining = _currentMatch.BallsRemaining - 1;
            var runsNeedToWin = _currentMatch.RunsNeedToWin;
            var currentStriker = striker;

            target.NextBall(_currentMatch, ref striker, ref nonStriker, currentOver);

            Assert.False(currentStriker.IsOut);
            Assert.True(currentStriker.IsOnField);
            Assert.Equal(runsNeedToWin, _currentMatch.RunsNeedToWin);
            Assert.Equal(expectedBallsRemaining, _currentMatch.BallsRemaining);
            //No change in striker
            Assert.Same(currentStriker, striker);
        }

    }
}
