using Leagues.Domain.Teams;

namespace Leagues.Domain.Leagues
{
    public class LeagueStanding
    {
        protected LeagueStanding()
        { }

        public LeagueStanding(League league, TeamAbbreviation team)
        {
            this.League = league;
            this.Team = team;
        }

        public int SurrogateId { get; private set; }

        public virtual League League { get; private set; }

        public TeamAbbreviation Team { get; private set; }

        public int Played { get; private set; }

        public int Points { get; private set; }

        public int PointsFor { get; private set; }

        public int PointsAgainst { get; private set; }

        public void RecordResult(Results result, int pointsFor, int pointsAgainst)
        {
            ++Played;
            PointsFor += pointsFor;
            PointsAgainst += pointsAgainst;

            switch (result)
            {
                case Results.Win:
                    Points += 3;
                    break;
                case Results.Draw:
                    Points += 1;
                    break;
                default: break;
            }
        }

        public enum Results
        {
            Win = 1,
            Draw = 2,
            Lose = 3
        }
    }
}
