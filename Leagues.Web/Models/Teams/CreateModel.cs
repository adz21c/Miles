using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Leagues.Web.Models.Teams
{
    public class CreateModel
    {
        [Required]
        [StringLength(10)]
        [RegularExpression("[a-zA-Z]+")]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}