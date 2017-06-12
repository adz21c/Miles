using Leagues.Domain.Leagues;
using Leagues.Domain.Teams;
using Miles.Persistence;
using System;
using System.Threading.Tasks;

namespace Leagues.Application
{
    public class FixtureManager
    {
        private readonly IRepository<League, LeagueAbbreviation> leagueRepository;

        public FixtureManager(IRepository<League, LeagueAbbreviation> leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        public async Task StartFixtureAsync(LeagueAbbreviation leagueId, Guid fixtureId, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.StartFixture(fixtureId, when);
            await leagueRepository.SaveAsync(league);
        }

        public async Task RecordGoalAsync(LeagueAbbreviation leagueId, Guid fixtureId, TeamAbbreviation team, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.RecordGoal(fixtureId, team, when);
            await leagueRepository.SaveAsync(league);
        }

        public async Task FinishFixtureAsync(LeagueAbbreviation leagueId, Guid fixtureId, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.FinishFixture(fixtureId, when);
            await leagueRepository.SaveAsync(league);
        }
    }
}
