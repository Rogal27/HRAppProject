using System;
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
        public string CompanyName { get; set; }
        public string Description { get; set; }

        public ICollection<JobOffers> JobOffers { get; set; }
    }
}
