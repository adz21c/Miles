using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leagues.Web.Models.Leagues
{
    public class StandingsModel
    {
        public List<StandingModelTeam> Teams { get; set; }
    }

    public class StandingModelTeam
    {
        public string Name { get; set; }

        public int Played { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public int PointsFor { get; set; }

        public int PointsAgainst { get; set; }

        public int Points { get; set; }
    }
}