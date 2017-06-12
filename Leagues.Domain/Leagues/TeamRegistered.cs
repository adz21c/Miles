using Leagues.Domain.Teams;

namespace Leagues.Domain.Leagues
{
    class TeamRegistered
    {
        public LeagueAbbreviation Id { get; set; }
        public TeamAbbreviation Team { get; set; }
    }
}