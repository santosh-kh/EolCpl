using System.Collections.Generic;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Contains properties to match mapData.json data.
    /// </summary>
    internal class EolCplData
    {
        public List<Match> Matches { get; set; }
        public List<Team> Teams { get; set; }
        public List<Player> Players { get; set; }
        public List<PlayerMatch> PlayerMatches { get; set; }
    }
}