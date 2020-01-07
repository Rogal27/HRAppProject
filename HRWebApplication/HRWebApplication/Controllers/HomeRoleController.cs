using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRWebApplication.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRWebApplication.Controllers
{
    public class HomeRoleController : Controller
    {
        public IActionResult Index()
        {
            if(User.IsInRole(UserRolesTypes.Admin)==true)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (User.IsInRole(UserRolesTypes.HR) == true)
            {
                return RedirectToAction("Index", "HR");
            }
            if (User.IsInRole(UserRolesTypes.Normal) == true)
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}