using Leagues.Domain.Teams;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Leagues.Application
{
    public class TeamManager
    {
        private readonly IRepository<Team, TeamAbbreviation> teamRepository;

        public TeamManager(IRepository<Team, TeamAbbreviation> teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task CreateTeam(TeamAbbreviation id, string name)
        {
            var team = new Team(id, name);
            await teamRepository.SaveAsync(team);
        }
    }
}
