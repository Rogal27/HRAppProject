using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRWebApplication.Helpers
{

    public static class UserRolesTypes
    {
        public static string Admin = "ADMIN";
        public static string HR = "HR";
        public static string Normal = "NORMAL_USER";
    }

    public static class ApplicationStatusState
    {
        public static string Pending = "PENDING";
        public static string Accepted = "ACCEPTED";
        public static string Rejected = "REJECTED";
    }

    public static class Globals
    {
        public static string IdClaimName = "UserIdClaim";
        public static List<string> getRoles = new List<string>() { "Admin", "Human Relations", "User" };
        public static string ValidJobOffer = "VALID";
    }

    public static class Helper
    {
        public static int GetUserId(ClaimsPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userId = claims.Where(x => x.Type == Globals.IdClaimName).First().Value;
            //int id = 1;
            int id = -1;
            if (int.TryParse(userId, out id) == false)
            {
                id = -1;
            }
            return id;
        }
    }
}
