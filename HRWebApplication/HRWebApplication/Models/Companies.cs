﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class Companies
    {
        public Companies()
        {
            JobOffers = new HashSet<JobOffers>();
        }

        public int CompanyId { get; set; }
        [Display(Name ="Company Name")]
        [Required]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public ICollection<JobOffers> JobOffers { get; set; }
    }
}
