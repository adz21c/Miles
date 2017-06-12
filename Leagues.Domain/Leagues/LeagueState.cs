using Leagues.Domain.Teams;
using Miles.Aggregates;
using System;
using System.Collections.Generic;

namespace Leagues.Domain.Leagues
{
    public class LeagueState :
        IState<LeagueAbbreviation>,
        IAppliesEvent<LeagueCreated>, IAppliesEvent<TeamRegistered>, IAppliesEvent<LeagueStarted>,
        IAppliesEvent<FixtureScheduled>, IAppliesEvent<FixtureStarted>, IAppliesEvent<GoalRecorded>, IAppliesEvent<FixtureFinished>
    {
        public LeagueAbbreviation Id { get; private set; }

        public LeagueStates State { get; private set; }

        private readonly List<TeamAbbreviation> registeredTeamsList = new List<TeamAbbreviation>();
        public IEnumerable<TeamAbbreviation> RegisteredTeams => registeredTeamsList;

        private readonly Dictionary<Guid, Fixture> fixtures = new Dictionary<Guid, Fixture>();
        public Fixture GetFixture(Guid id) { return fixtures[id]; }

        void IAppliesEvent<LeagueCreated>.ApplyEvent(LeagueCreated @event)
        {
            Id = @event.Id;
            State = LeagueStates.Planning;
        }

        void IAppliesEvent<TeamRegistered>.ApplyEvent(TeamRegistered @event)
        {
            registeredTeamsList.Add(@event.Team);
        }

        void IAppliesEvent<LeagueStarted>.ApplyEvent(LeagueStarted @event)
        {
            State = LeagueStates.InProgress;
        }

        void IAppliesEvent<FixtureScheduled>.ApplyEvent(FixtureScheduled @event)
        {
            fixtures.Add(
                @event.Id,
                new Fixture
                {
                    Id = @event.Id,
                    State = FixtureStates.Scheduled,
                    TeamA = @event.TeamA,
                    TeamB = @event.TeamB,
                    ScheduledDateTime = @event.ScheduledDateTime
                });
        }

        void IAppliesEvent<FixtureStarted>.ApplyEvent(FixtureStarted @event)
        {
            var fixture = GetFixture(@event.Id);
            fixture.Started = @event.When;
            fixture.State = FixtureStates.InProgress;
        }

        void IAppliesEvent<GoalRecorded>.ApplyEvent(GoalRecorded @event)
        {
            var fixture = GetFixture(@event.Id);
            if (fixture.TeamA == @event.Team)
                fixture.TeamAPoints += 1;
            else
                fixture.TeamBPoints += 1;
        }

        void IAppliesEvent<FixtureFinished>.ApplyEvent(FixtureFinished @event)
        {
            var fixture = GetFixture(@event.Id);
            fixture.Finished = @event.When;
            fixture.State = FixtureStates.Finished;
        }
    }
}
