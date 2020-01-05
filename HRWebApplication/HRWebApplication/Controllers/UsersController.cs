using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;

namespace HRWebApplication.Controllers
{
    public class UsersController : Controller
    {
        //OLD
        //public IActionResult SignIn()
        //{
        //    return View();
        //}


        //public IActionResult SignUp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SignUp([Bind("Email,FirstName,LastName")] Users user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = _context.UserRoles.FirstOrDefault(u => u.Role == "NORMAL_USER");
        //        user.UserRoleId = role.UserRoleId;
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Users));
        //    }
        //    //ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "UserRoleId", "Role", user.UserRoleId);
        //    return View(user);
        //}
    }
}
