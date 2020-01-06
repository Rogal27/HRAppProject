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
        [Authorize(Roles = "NORMAL_USER")]
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
        [Authorize(Roles = "NORMAL_USER")]
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
            int userId = Helper.GetUserId(User);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == userId);
            if(user == null)
            {
                return NotFound();
            }
            var api = await _context.Applications.FirstOrDefaultAsync(a => a.UserId == userId && a.JobOfferId == jobOffer.JobOfferId);
            if (api != null)
            {
                return RedirectToAction("ApplicationInfo", "Home");
            }


            var application = new ApplicationCreateModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                JobOfferId = jobOffer.JobOfferId,
                UserId = user.UserId
            };
            ViewData["JobTitle"] = jobOffer.JobTitle;

            return View(application);
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NORMAL_USER")]
        public async Task<IActionResult> Create([Bind("ApplicationId,JobOfferId,UserId,FirstName,LastName,Email,Phone,BirthDate,Cvid,CVFile,OtherAttachmentsFiles")] ApplicationCreateModel applications)
        {
            if (ModelState.IsValid)
            {
                var api = await _context.Applications.FirstOrDefaultAsync(a => a.UserId == applications.UserId && a.JobOfferId == applications.JobOfferId);
                if (api != null)
                {
                    return RedirectToAction("ApplicationInfo", "Home");
                }
                var id = Helper.GetUserId(User);
                if (applications.UserId != id)
                {
                    return Unauthorized();
                }
                var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(a => a.JobOfferId == applications.JobOfferId);
                if (jobOffer == null)
                {
                    return NotFound();
                }

                var pendingId = await _context.ApplicationStatus.FirstOrDefaultAsync(x => x.Status == ApplicationStatusState.Pending);
                applications.ApplicationStatusId = pendingId.ApplicationStatusId;
                _context.Add(applications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NORMAL_USER")]
        public IActionResult UploadCV()
        {
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NORMAL_USER")]
        public IActionResult UploadAttachments()
        {
            return NotFound();
        }

        // GET: Applications/Edit/5
        [Authorize(Roles = "NORMAL_USER")]
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
        [Authorize(Roles = "NORMAL_USER")]
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
        [Authorize(Roles = "NORMAL_USER")]
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
        [Authorize(Roles = "NORMAL_USER")]
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
