using System;

namespace Leagues.Domain.Leagues
{
    class LeagueStarted
    {
        public LeagueAbbreviation Id { get; set; }
        public DateTime When { get; set; }
    }
}