using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int? Cvid { get; set; }
        public int ApplicationStatusId { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
        public CV Cv { get; set; }
        public JobOffers JobOffer { get; set; }
        public Users User { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationAttachment> ApplicationAttachment { get; set; }
    }
}
