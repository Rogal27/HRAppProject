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
    public class ApplicationsController : Controller
    {
        private readonly HRProjectDatabaseContext _context;

        public ApplicationsController(HRProjectDatabaseContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var hRProjectDatabaseContext = _context.Applications.Include(a => a.ApplicationStatus).Include(a => a.Cv).Include(a => a.JobOffer).Include(a => a.User);
            return View(await hRProjectDatabaseContext.ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applications = await _context.Applications
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Cv)
                .Include(a => a.JobOffer)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (applications == null)
            {
                return NotFound();
            }

            return View(applications);
        }

        // GET: Applications/Create/5
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(a => a.JobOfferId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }
            ViewData["JobTitle"] = jobOffer.JobTitle;
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "ApplicationStatusId", "Status");
            ViewData["Cvid"] = new SelectList(_context.Cv, "CVID", "Path");
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "JobOfferId", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,JobOfferId,UserId,FirstName,LastName,Email,Phone,BirthDate,Cvid,ApplicationStatusId")] Applications applications)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "ApplicationStatusId", "Status", applications.ApplicationStatusId);
            ViewData["Cvid"] = new SelectList(_context.Cv, "CVID", "Path", applications.Cvid);
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "JobOfferId", "Description", applications.JobOfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", applications.UserId);
            return View(applications);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applications = await _context.Applications.FindAsync(id);
            if (applications == null)
            {
                return NotFound();
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "ApplicationStatusId", "Status", applications.ApplicationStatusId);
            ViewData["Cvid"] = new SelectList(_context.Cv, "CVID", "Path", applications.Cvid);
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "JobOfferId", "Description", applications.JobOfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", applications.UserId);
            return View(applications);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,JobOfferId,UserId,FirstName,LastName,Email,Phone,BirthDate,Cvid,ApplicationStatusId")] Applications applications)
        {
            if (id != applications.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationsExists(applications.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "ApplicationStatusId", "Status", applications.ApplicationStatusId);
            ViewData["Cvid"] = new SelectList(_context.Cv, "CVID", "Path", applications.Cvid);
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "JobOfferId", "Description", applications.JobOfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", applications.UserId);
            return View(applications);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applications = await _context.Applications
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Cv)
                .Include(a => a.JobOffer)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (applications == null)
            {
                return NotFound();
            }

            return View(applications);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applications = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(applications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationsExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }
    }
}
