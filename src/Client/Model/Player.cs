
namespace Eol.Cpl.Client
{
    /// <summary>
    /// Conatins properties to hold information about player.
    /// </summary>
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public int[] ScoreProbabilities { get; set; }
    }
}
