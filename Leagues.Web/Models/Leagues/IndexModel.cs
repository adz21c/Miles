using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leagues.Web.Models.Teams
{
    public class IndexModel
    {
        public List<IndexModelTeam> Teams { get; set; }
    }

    public class IndexModelTeam
    {
        public string Abbreviation { get; set; }

        public string Name { get; set; }
    }
}