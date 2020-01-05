using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRWebApplication.Helpers
{

    public static class UserRolesTypes
    {
        public static string Admin = "ADMIN";
        public static string HR = "HR";
        public static string Normal = "NORMAL_USER";
    }

    public static class Globals
    {
        public static string IdClaimName = "UserIdClaim";
    }
}
