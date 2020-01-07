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
        public async Task<IActionResult> GetApplications(int? pageNumber, int? pageSize, int? companyId, string searchString, string currentFilter, string sortOrder)
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
            ViewData["CompanyId"] = companyId;

            int userId = Helper.GetUserId(User);

            var companynames = _context.Applications.Include(j => j.JobOffer).Where(j => j.UserId == userId).Include(j=>j.JobOffer.Company).Select(j => new { title = j.JobOffer.Company.CompanyName, id = j.JobOffer.CompanyId });
            var CompanyList = companynames.Distinct().OrderBy(x => x.title);
            ViewBag.CompanyNames = new SelectList(CompanyList, "id", "title");

            var applications = _context.Applications.Include(j => j.JobOffer).Include(j => j.ApplicationStatus).Include(j => j.JobOffer.Company).Where(j => j.UserId == userId);
            IQueryable<Applications> applications_filter = applications;

            if (companyId.HasValue == true)
            {
                applications_filter = applications_filter.Where(x => x.JobOffer.CompanyId == companyId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                applications_filter = applications_filter.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString) || x.Email.Contains(searchString) || (x.FirstName + " " + x.LastName).Contains(searchString) || x.JobOffer.JobTitle.Contains(searchString) || x.JobOffer.Company.CompanyName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.JobOffer.JobTitle).ThenByDescending(x => x.ApplicationId);
                    break;
                case "title_asc":
                    applications_filter = applications_filter.OrderBy(x => x.JobOffer.JobTitle).ThenByDescending(x => x.ApplicationId);
                    break;
                case "cname_desc":
                    applications_filter = applications_filter.OrderByDescending(x => x.JobOffer.Company.CompanyName);
                    break;
                case "cname_asc":
                    applications_filter = applications_filter.OrderBy(x => x.JobOffer.Company.CompanyName);
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

            int userId = Helper.GetUserId(User);
            var applications = await _context.Applications
                .Include(j => j.JobOffer)
                .Include(j => j.ApplicationStatus)
                .FirstOrDefaultAsync(j => j.ApplicationId == id && j.UserId == userId);

            if (applications == null)
            {
                return Unauthorized();
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
            var userId = Helper.GetUserId(User);
            var api = await _context.Applications
                .Include(x => x.JobOffer)
                .Include(x => x.ApplicationStatus)
                .FirstOrDefaultAsync(x => x.ApplicationId == id && x.UserId == userId);
            if (api == null)
            {
                return NotFound();
            }


            var application = new ApplicationCreateModel()
            {
                FirstName = api.FirstName,
                LastName = api.LastName,
                Email = api.Email,
                JobOffer = api.JobOffer,
                JobOfferId = api.JobOffer.JobOfferId,
                UserId = api.UserId,
                ApplicationId = api.ApplicationId,
                ApplicationStatus = api.ApplicationStatus,
                ApplicationStatusId = api.ApplicationStatusId,
                BirthDate = api.BirthDate,
                Phone = api.Phone
            };
            ViewData["JobTitle"] = api.JobOffer.JobTitle;

            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Applications/[action]")]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,ApplicationStatusId,JobOfferId,UserId,FirstName,LastName,Email,Phone,BirthDate,Cvid,CVFile,OtherAttachmentsFiles")] ApplicationCreateModel applications)
        {
            if (id != applications.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = Helper.GetUserId(User);
                if (applications.UserId != userId)
                {
                    return Unauthorized();
                }
                var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(a => a.JobOfferId == applications.JobOfferId);
                if (jobOffer == null)
                {
                    return NotFound();
                }
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
            if (applications.JobOffer == null)
            {
                ViewData["JobTitle"] = "Job offer title unavailable";
            }
            else
            {
                ViewData["JobTitle"] = applications.JobOffer?.JobTitle;
            }
            
            return View(applications);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Applications/[action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = Helper.GetUserId(User);
            var applications = await _context.Applications.FirstOrDefaultAsync(x => x.ApplicationId == id && x.UserId == userId);
            if(applications == null)
            {
                return NotFound();
            }

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