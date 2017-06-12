using Leagues.Domain.Teams;
using System;

namespace Leagues.Domain.Leagues
{
    class GoalRecorded
    {
        public LeagueAbbreviation League { get; set; }

        public Guid Id { get; set; }

        public DateTime When { get; set; }

        public TeamAbbreviation Team { get; set; }
    }
}