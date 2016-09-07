﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Miles.Sample.Web.Models.Leagues
{
    public class CreateModel
    {
        [Required]
        [StringLength(6)]
        [RegularExpression("[a-zA-Z]+")]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}