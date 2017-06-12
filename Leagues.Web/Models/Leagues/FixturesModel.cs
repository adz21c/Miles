using System;
using System.Collections.Generic;

namespace Leagues.Web.Models.Leagues
{
    public class FixturesModel
    {
        public string LeagueId { get; set; }

        public List<string> Teams { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }

        public DateTime ScheduledDateTime { get; set; }

        public List<FixturesModelFixture> Fixtures { get; set; }
    }

    public class FixturesModelFixture
    {
        public Guid Id { get; set; }

        public string TeamA { get; set; }

        public int? TeamAPoints { get; set; }

        public string TeamB { get; set; }

        public int? TeamBPoints { get; set; }

        public DateTime ScheduledDateTime { get; set; }

        public bool Active { get; set; }

        public DateTime? Completed { get; set; }
    }
}