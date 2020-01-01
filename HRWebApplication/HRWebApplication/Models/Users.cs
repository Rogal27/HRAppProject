using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRWebApplication.Models
{
    public partial class Users
    {
        public Users()
        {
            Applications = new HashSet<Applications>();
            HRJobOffers = new HashSet<HrjobOffers>();
        }

        public int UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }

        public UserRoles UserRole { get; set; }
        public ICollection<Applications> Applications { get; set; }
        public ICollection<HrjobOffers> HRJobOffers { get; set; }
    }
}
