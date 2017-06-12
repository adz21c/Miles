using Miles.Aggregates;
using System;

namespace Leagues.Domain.Teams
{
    public class Team : Aggregate<TeamState, TeamAbbreviation>
    {
        public Team()
        { }

        public Team(TeamAbbreviation id, string name)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.ApplyNewEvent(new TeamCreated
            {
                Id = id,
                Name = name
            });
        }
    }
}
