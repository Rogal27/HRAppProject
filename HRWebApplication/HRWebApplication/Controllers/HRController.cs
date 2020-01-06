using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly HRProjectDatabaseContext _context;

        public HRController(HRProjectDatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult YourJobOffers()
        {
            int id = Helper.GetUserId(User);
            ViewData["UserID"] = id;
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

            var jobtitles = _context.Applications.Include(j => j.JobOffer).Where(j => j.JobOffer.UserId == userId).Select(j => new { title = j.JobOffer.JobTitle, id = j.JobOfferId });                           

            ViewBag.JobTitles = new SelectList(jobtitles,"id","title");

            var applications = _context.Applications.Include(j => j.JobOffer).Include(j => j.ApplicationStatus).Where(j => j.JobOffer.UserId == userId);
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

        [Route("[controller]/Applications/[action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int userId = Helper.GetUserId(User);
            var applications = await _context.Applications.Include(j => j.JobOffer).Include(j => j.ApplicationStatus).FirstOrDefaultAsync(j => j.ApplicationId == id && j.JobOffer.UserId == userId);

            if (applications == null)
            {
                return Unauthorized();
            }

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Applications/[action]")]
        public async Task<IActionResult> ChangeStatus(int? id, string status)
        {
            if(string.IsNullOrEmpty(status)==true || id.HasValue == false)
            {
                return NotFound();
            }
            int userId = Helper.GetUserId(User);
            var applications = await _context.Applications.Include(j => j.JobOffer).Include(j => j.ApplicationStatus).FirstOrDefaultAsync(j => j.ApplicationId == id && j.JobOffer.UserId == userId);

            if (applications == null)
            {
                return Unauthorized();
            }

            if (applications.ApplicationStatus.Status != ApplicationStatusState.Pending)
            {
                return BadRequest();
            }
            if (status != ApplicationStatusState.Accepted && status != ApplicationStatusState.Rejected)
            {
                return BadRequest();
            }
            var new_status = await _context.ApplicationStatus.FirstOrDefaultAsync(j => j.Status == status);

            if(new_status == null)
            {
                return BadRequest();
            }

            applications.ApplicationStatusId = new_status.ApplicationStatusId;

            _context.Update(applications);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = applications.ApplicationId });
        }
    }
}