using System;
using System.Collections.Generic;

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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }

        public UserRoles UserRole { get; set; }
        public ICollection<Applications> Applications { get; set; }
        public ICollection<HrjobOffers> HRJobOffers { get; set; }
    }
}
