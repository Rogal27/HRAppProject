using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class Applications
    {
        public Applications()
        {
            ApplicationAttachment = new HashSet<ApplicationAttachment>();
        }

        public int ApplicationId { get; set; }
        public int JobOfferId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int? Cvid { get; set; }
        public int ApplicationStatusId { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
        public CV Cv { get; set; }
        public JobOffers JobOffer { get; set; }
        public Users User { get; set; }
        public ICollection<ApplicationAttachment> ApplicationAttachment { get; set; }
    }
}
