using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using HRWebApplication.Helpers;

namespace HRWebApplication.Controllers
{
    [Authorize]
    public class UsersDataController : Controller
    {
        private readonly HRProjectDatabaseContext _context;

        public UsersDataController(HRProjectDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = Helper.GetUserId(User);

            if (userId != id)
            {
                return Unauthorized();
            }

            var users = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }
    }
}
