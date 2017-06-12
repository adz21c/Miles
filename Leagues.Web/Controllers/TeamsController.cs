using Leagues.Application;
using Leagues.Domain.Teams;
using Leagues.Web.Models.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Leagues.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamManager teamManager;

        public TeamsController(TeamManager teamManager)
        {
            this.teamManager = teamManager;
        }

        // GET: Team
        public ActionResult Index()
        {
            // TODO: List of teams
            return View(new List<IndexModelTeam>());
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View(new CreateModel());
        }

        // POST: Team/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await teamManager.CreateTeam(TeamAbbreviation.Parse(model.Abbreviation), model.Name);

            return RedirectToAction("Index");
        }
    }
}
