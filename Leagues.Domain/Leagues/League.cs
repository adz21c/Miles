using Leagues.Domain.Teams;
using Miles.Aggregates;
using System;
using System.Linq;

namespace Leagues.Domain.Leagues
{
    public class League : Aggregate<LeagueState, LeagueAbbreviation>
    {
        public League()
        { }

        public League(LeagueAbbreviation abbreviation, string name)
        {
            if (abbreviation == null)
                throw new ArgumentNullException(nameof(abbreviation));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.ApplyNewEvent(new LeagueCreated
            {
                Id = abbreviation,
                Name = name
            });
        }

        public void RegisterTeam(TeamAbbreviation team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            if (State.State != LeagueStates.Planning)
                throw new InvalidOperationException("The league is not open for planning");

            if (State.RegisteredTeams.Contains(team))
                return;

            this.ApplyNewEvent(new TeamRegistered
            {
                Id = State.Id,
                Team = team
            });
        }

        public void StartLeague(DateTime when)
        {
            if (State.State == LeagueStates.InProgress)
                return;

            if (State.State == LeagueStates.Completed)
                throw new InvalidOperationException("League has already completed");

            this.ApplyNewEvent(new LeagueStarted
            {
                Id = State.Id,
                When = when
            });
        }

        public Guid ScheduleFixture(TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            if (teamA == null)
                throw new ArgumentNullException(nameof(teamA));
            if (teamB == null)
                throw new ArgumentNullException(nameof(teamB));
            if (teamA == teamB)
                throw new ArgumentOutOfRangeException(nameof(teamB), "Team A and Team B cannot be the same team");

            if (State.State != LeagueStates.Planning)
                throw new InvalidOperationException("League is not open to scheduling");

            if (!State.RegisteredTeams.Contains(teamA))
                throw new ArgumentOutOfRangeException(nameof(teamA), "Not registered with the league");
            if (!State.RegisteredTeams.Contains(teamB))
                throw new ArgumentOutOfRangeException(nameof(teamA), "Not registered with the league");

            var fixtureId = Guid.NewGuid();
            this.ApplyNewEvent(new FixtureScheduled
            {
                Id = fixtureId,
                League = State.Id,
                TeamA = teamA,
                TeamB = teamB,
                ScheduledDateTime = scheduledDateTime
            });
            return fixtureId;
        }

        public void StartFixture(Guid fixtureId, DateTime when)
        {
            var fixture = State.GetFixture(fixtureId);
            if (fixture == null)
                throw new InvalidOperationException("Could not find the Fixture");

            if (fixture.State != FixtureStates.Scheduled)
                throw new InvalidOperationException("Fixture has already started or completed");

            if (State.State != LeagueStates.InProgress)
                throw new InvalidOperationException("The league is not in-progress");

            this.ApplyNewEvent(new FixtureStarted
            {
                Id = fixtureId,
                League = State.Id,
                When = when
            });
        }

        public void RecordGoal(Guid fixtureId, TeamAbbreviation team, DateTime when)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            var fixture = State.GetFixture(fixtureId);
            if (fixture == null)
                throw new InvalidOperationException("Could not find the Fixture");

            if (team != fixture.TeamA && team != fixture.TeamB)
                throw new ArgumentOutOfRangeException(nameof(team), "Must be one of the fixture's teams");

            if (fixture.State != FixtureStates.InProgress)
                throw new InvalidOperationException("Cannot score a goal while the game is not in-progress");

            if (when < fixture.Started.Value)
                throw new ArgumentOutOfRangeException(nameof(when), "Cannot score a goal before the game has started.");

            this.ApplyNewEvent(new GoalRecorded
            {
                League = State.Id,
                Id = fixtureId,
                When = when,
                Team = team
            });
        }

        public void FinishFixture(Guid fixtureId, DateTime when)
        {
            var fixture = State.GetFixture(fixtureId);
            if (fixture == null)
                throw new InvalidOperationException("Could not find the Fixture");

            if (fixture.State != FixtureStates.InProgress)
                throw new InvalidOperationException("Fixture has not started or has already completed");

            if (when <= fixture.Started.Value)
                throw new ArgumentOutOfRangeException(nameof(when), "Cannot finish the fixture before it started.");

            FixtureResults result;
            if (fixture.TeamAPoints > fixture.TeamBPoints)
                result = FixtureResults.TeamAWins;
            else if (fixture.TeamAPoints < fixture.TeamBPoints)
                result = FixtureResults.TeamBWins;
            else
                result = FixtureResults.Draw;

            this.ApplyNewEvent(new FixtureFinished
            {
                League = State.Id,
                Id = fixture.Id,
                When = when,
                Result = result
            });
        }
    }
}
