using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class JobOffers
    {
        public JobOffers()
        {
            Applications = new HashSet<Applications>();
            HRJobOffers = new HashSet<HrjobOffers>();
        }

        public int JobOfferId { get; set; }
        public string JobTitle { get; set; }
        public int? SalaryFrom { get; set; }
        public int? SalaryTo { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int JobOfferStatusId { get; set; }
        public int CompanyId { get; set; }

        public Companies Company { get; set; }
        public JobOfferStatus JobOfferStatus { get; set; }
        public ICollection<Applications> Applications { get; set; }
        public ICollection<HrjobOffers> HRJobOffers { get; set; }
    }
}
