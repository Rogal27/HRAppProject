using System;
using System.Collections.Generic;

namespace HRWebApplication.Models
{
    public partial class UserRoles
    {
        public UserRoles()
        {
            Users = new HashSet<Users>();
        }

        public int UserRoleId { get; set; }
        public string Role { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
