using System;

namespace Leagues.Domain.Leagues
{
    public class FixtureFinished
    {
        public Guid Id { get; set; }

        public LeagueAbbreviation League { get; set; }

        public DateTime When { get; set; }

        public FixtureResults Result { get; set; }
    }
}