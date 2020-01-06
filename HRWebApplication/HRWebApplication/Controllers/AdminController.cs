using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRWebApplication.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        
        private readonly HRProjectDatabaseContext _context;

        public AdminController(HRProjectDatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Admin/Users
        [Route("[controller]/Users/[action]")]
        public async Task<IActionResult> GetUsers()
        {
            var hRProjectDatabaseContext = _context.Users.Include(u => u.UserRole);
            return View(await hRProjectDatabaseContext.ToListAsync());
        }

        // GET: Admin/Users/Details/5
        [Route("[controller]/Users/[action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
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

        // GET: Admin/Users/Create
        //[Route("[controller]/Users/[action]")]
        //public IActionResult Create()
        //{
        //    ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "UserRoleId", "Role");
        //    return View();
        //}

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("[controller]/Users/[action]")]
        //public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,UserRoleId")] Users users)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(users);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Users));
        //    }
        //    ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "UserRoleId", "Role", users.UserRoleId);
        //    return View(users);
        //}

        // GET: Admin/Users/Edit/5
        [Route("[controller]/Users/[action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "UserRoleId", "Role", users.UserRoleId);
            return View(users);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Users/[action]")]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,UserRoleId")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetUsers));
            }
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "UserRoleId", "Role", users.UserRoleId);
            return View(users);
        }

        // GET: Admin/Users/Delete/5
        //[Route("[controller]/Users/[action]")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users
        //        .Include(u => u.UserRole)
        //        .FirstOrDefaultAsync(m => m.UserId == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Users/[action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetUsers));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}