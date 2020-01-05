using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRWebApplication.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRWebApplication.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult YourJobOffers()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userId = claims.Where(x => x.Type == Globals.IdClaimName).First().Value;
            //int id = 1;
            int id = 0;
            if (int.TryParse(userId, out id) == false)
            {
                id = 0;
            }
            ViewData["UserID"] = id;
            return View();
        }
    }
}