using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Job Title")]
        [Required]
        public string JobTitle { get; set; }
        [Display(Name = "Salary From")]
        [DataType(DataType.Text)]
        public int? SalaryFrom { get; set; }
        [Display(Name = "Salary To")]
        [DataType(DataType.Text)]
        public int? SalaryTo { get; set; }
        public string Location { get; set; }
        [Display(Name = "Offer creation date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }
        [Display(Name = "Offer expiration date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ValidUntil { get; set; }
        public int JobOfferStatusId { get; set; }
        public int CompanyId { get; set; }

        public Companies Company { get; set; }
        public JobOfferStatus JobOfferStatus { get; set; }
        public ICollection<Applications> Applications { get; set; }
        public ICollection<HrjobOffers> HRJobOffers { get; set; }
    }
}
