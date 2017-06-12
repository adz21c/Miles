using Leagues.Application;
using Leagues.Domain.Leagues;
using Leagues.Domain.Teams;
using Leagues.Web.Models.Leagues;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Leagues.Web.Controllers
{
    public class FixturesController : Controller
    {
        private readonly LeagueManager leagueManager;
        private readonly FixtureManager fixtureManager;

        public FixturesController(
            LeagueManager leagueManager,
            FixtureManager fixtureManager)
        {
            this.leagueManager = leagueManager;
            this.fixtureManager = fixtureManager;
        }

        [HttpPost]
        public async Task<ActionResult> Schedule(string leagueId, FixturesModel model)
        {
            var league = LeagueAbbreviation.Parse(leagueId);
            var teamA = TeamAbbreviation.Parse(model.TeamA);
            var teamB = TeamAbbreviation.Parse(model.TeamB);
            var scheduledDateTime = model.ScheduledDateTime;

            await leagueManager.ScheduleFixtureAsync(league, teamA, teamB, scheduledDateTime);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> Start(string leagueId, Guid fixtureId)
        {
            var league = LeagueAbbreviation.Parse(leagueId);
            await fixtureManager.StartFixtureAsync(league, fixtureId, DateTime.Now);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> RecordGoal(string leagueId, Guid fixtureId, string teamId, int points)
        {
            var league = LeagueAbbreviation.Parse(leagueId);
            var team = TeamAbbreviation.Parse(teamId);

            await fixtureManager.RecordGoalAsync(league, fixtureId, team, DateTime.Now);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> Finish(string leagueId, Guid fixtureId)
        {
            var league = LeagueAbbreviation.Parse(leagueId);

            await fixtureManager.FinishFixtureAsync(league, fixtureId, DateTime.Now);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }
    }
}