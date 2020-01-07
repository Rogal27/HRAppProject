using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRWebApplication.Helpers;
using HRWebApplication.Models;
using HRWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRWebApplication.Controllers
{
    [Authorize(Roles = "NORMAL_USER")]
    public class UserController : Controller
    {
        private readonly HRProjectDatabaseContext _context;

        public UserController(HRProjectDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[controller]/Applications/[action]")]
        public async Task<IActionResult> GetApplications(int? pageNumber, int? pageSize, int? jobId, string searchString, string currentFilter, string sortOrder)
        {
            if (pageNumber.HasValue == false || pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize.HasValue == false || pageSize < 1)
            {
                pageSize = 10;
            }
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["JobId"] = jobId;

            int userId = Helper.GetUserId(User);

            var jobtitles = _context.Applications.Include(j => j.JobOffer).Where(j => j.UserId == userId).Select(j => new { title = j.JobOffer.JobTitle, id = j.JobOfferId });

            ViewBag.JobTitles = new SelectList(jobtitles, "id", "title");

            var applications = _context.Applications.Include(j => j.JobOffer).Include(j => j.ApplicationStatus).Where(j => j.UserId == userId);
            IQueryable<Applications> applications_filter = applications;

            if (jobId.HasValue == true)
            {
                applications_filter = applications_filter.Where(x => x.JobOfferId == jobId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                applications_filter = applications_filter.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString) || x.Email.Contains(searchString) || (x.FirstName + " " + x.LastName).Contains(searchString) || x.JobOffer.JobTitle.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.JobOffer.JobTitle).ThenByDescending(x => x.ApplicationId);
                    break;
                case "title_asc":
                    applications_filter = applications_filter.OrderBy(x => x.JobOffer.JobTitle).ThenByDescending(x => x.ApplicationId);
                    break;
                case "fname_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.FirstName);
                    break;
                case "fname_asc":
                    applications_filter = applications_filter.OrderBy(x => x.FirstName);
                    break;
                case "lname_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.LastName);
                    break;
                case "lname_asc":
                    applications_filter = applications_filter.OrderBy(x => x.LastName);
                    break;
                case "status_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.ApplicationStatus.Status);
                    break;
                case "status_asc":
                    applications_filter = applications_filter.OrderBy(x => x.ApplicationStatus.Status);
                    break;
                default:
                    applications_filter = applications_filter.OrderByDescending(x => x.ApplicationId);
                    break;
            }

            return View(await PaginatedList<Applications>.CreateAsync(applications_filter.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10));
        }

        // GET: Applications/Create/5
        [Route("[controller]/Applications/[action]")]
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
            if (user == null)
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
        [Route("[controller]/Applications/[action]")]
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


        // GET: Applications/Details/5
        [Route("[controller]/Applications/[action]")]
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
        // GET: Applications/Edit/5
        [Route("[controller]/Applications/[action]")]
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
        [Route("[controller]/Applications/[action]")]
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
        [Route("[controller]/Applications/[action]")]
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
        [Route("[controller]/Applications/[action]")]
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