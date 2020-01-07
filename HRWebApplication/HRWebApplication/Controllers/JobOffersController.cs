using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;
using HRWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HRWebApplication.Helpers;

namespace HRWebApplication.Controllers
{
    public class JobOffersController : Controller
    {
        private readonly HRProjectDatabaseContext _context;

        public JobOffersController(HRProjectDatabaseContext context)
        {
            _context = context;
        }

        // GET: JobOffers
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: JobOffers/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobOffers jobOffers;
            if(User.IsInRole(UserRolesTypes.Admin)==true)
            {
                jobOffers = await _context.JobOffers
                .Include(j => j.Company)
                .Include(j => j.JobOfferStatus)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.JobOfferId == id);
            }
            else
            {
                jobOffers = await _context.JobOffers
                .Include(j => j.Company)
                .Include(j => j.JobOfferStatus)
                .FirstOrDefaultAsync(m => m.JobOfferId == id);
            }
            
            if (jobOffers == null)
            {
                return NotFound();
            }
            return View(jobOffers);
        }

        // GET: JobOffers/Create
        [HttpGet]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCompanyModel
            {
                CompaniesCollection = await _context.Companies.ToListAsync()
            };

            return View(model);
        }

        // POST: JobOffers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Create([Bind("JobOfferId,JobTitle,SalaryFrom,SalaryTo,Location,Description,ValidUntil,CompanyId")] JobOfferCompanyModel data)
        {
            //if (!ModelState.IsValid)
            //{
            //    data.CompaniesCollection = await _context.Companies.ToListAsync();
            //    return View(data);
            //}
            bool valid = true;

            if(data!=null)
            {
                var comp = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == data.CompanyId);
                if (comp == null)
                    valid = false;
                if (string.IsNullOrEmpty(data.JobTitle) == true)
                    valid = false;
                if (string.IsNullOrEmpty(data.Description) == true || data.Description.Length < 5)
                    valid = false;
                if (data.SalaryFrom.HasValue && data.SalaryFrom < 0)
                    valid = false;
                if (data.SalaryTo.HasValue && data.SalaryTo < 0)
                    valid = false;
                if (data.SalaryFrom.HasValue && data.SalaryTo.HasValue && data.SalaryFrom > data.SalaryTo)
                    valid = false;
                if (data.ValidUntil.HasValue == true)
                {
                    if (data.ValidUntil < DateTime.Now)
                        valid = false;
                }
            }

            if(valid ==false)
            {
                data.CompaniesCollection = await _context.Companies.ToListAsync();
                return BadRequest();
            }


            int userID = Helper.GetUserId(User);
            if (userID == -1)
                return Unauthorized();

            var status = _context.JobOfferStatus.FirstOrDefault(stat => stat.Status == "VALID");
            JobOffers job = new JobOffers
            {
                CompanyId = data.CompanyId,
                CreationDate = DateTime.Now,
                Description = data.Description,
                JobOfferStatusId = status.JobOfferStatusId,
                JobTitle = data.JobTitle,
                Location = data.Location,
                SalaryFrom = data.SalaryFrom,
                SalaryTo = data.SalaryTo,
                UserId = userID,
                ValidUntil = data.ValidUntil
            };
            



            await _context.JobOffers.AddAsync(job);
            await _context.SaveChangesAsync();

            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return Json("Message sent!");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "HR")]
        //public async Task<IActionResult> Create([Bind("JobOfferId,JobTitle,SalaryFrom,SalaryTo,Location,Description,ValidUntil,CompanyId")] JobOfferCompanyModel data)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    data.CompaniesCollection = await _context.Companies.ToListAsync();
        //    //    return View(data);
        //    //}
        //    bool valid = true;

        //    if (data != null)
        //    {
        //        var comp = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == data.CompanyId);
        //        if (comp == null)
        //            valid = false;
        //        if (string.IsNullOrEmpty(data.JobTitle) == true)
        //            valid = false;
        //        if (string.IsNullOrEmpty(data.Description) == true || data.Description.Length < 5)
        //            valid = false;
        //        if (data.SalaryFrom.HasValue && data.SalaryFrom < 0)
        //            valid = false;
        //        if (data.SalaryTo.HasValue && data.SalaryTo < 0)
        //            valid = false;
        //        if (data.SalaryFrom.HasValue && data.SalaryTo.HasValue && data.SalaryFrom > data.SalaryTo)
        //            valid = false;
        //        if (data.ValidUntil.HasValue == true)
        //        {
        //            if (data.ValidUntil < DateTime.Now)
        //                valid = false;
        //        }
        //    }

        //    if (valid == false)
        //    {
        //        data.CompaniesCollection = await _context.Companies.ToListAsync();
        //        return View(data);
        //    }


        //    int userID = Helper.GetUserId(User);
        //    if (userID == -1)
        //        return Unauthorized();

        //    var status = _context.JobOfferStatus.FirstOrDefault(stat => stat.Status == "VALID");
        //    JobOffers job = new JobOffers
        //    {
        //        CompanyId = data.CompanyId,
        //        CreationDate = DateTime.Now,
        //        Description = data.Description,
        //        JobOfferStatusId = status.JobOfferStatusId,
        //        JobTitle = data.JobTitle,
        //        Location = data.Location,
        //        SalaryFrom = data.SalaryFrom,
        //        SalaryTo = data.SalaryTo,
        //        UserId = userID,
        //        ValidUntil = data.ValidUntil
        //    };




        //    await _context.JobOffers.AddAsync(job);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("YourJobOffers", "HR");
        //}

        // GET: JobOffers/Edit/5
        [Authorize(Roles = "HR")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobOffers = await _context.JobOffers.FindAsync(id);
            if (jobOffers == null)
            {
                return NotFound();
            }
            int userID = Helper.GetUserId(User);
            if (userID == -1 || jobOffers.UserId != userID)
                return Unauthorized();

            var model = new JobOfferCompanyModel
            {
                CompaniesCollection = await _context.Companies.ToListAsync(),
                CompanyId = jobOffers.CompanyId,
                CreationDate = jobOffers.CreationDate,
                Description = jobOffers.Description,
                JobOfferId = jobOffers.JobOfferId,
                JobOfferStatusId = jobOffers.JobOfferStatusId,
                JobTitle = jobOffers.JobTitle,
                Location = jobOffers.Location,
                SalaryFrom = jobOffers.SalaryFrom,
                SalaryTo = jobOffers.SalaryTo,
                UserId = jobOffers.UserId,
                ValidUntil = jobOffers.ValidUntil                
            };
            ViewData["JobOfferStatusId"] = new SelectList(_context.JobOfferStatus, "JobOfferStatusId", "Status", jobOffers.JobOfferStatusId);
            return View(model);
        }

        // POST: JobOffers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Edit(int id, [Bind("JobOfferId,JobTitle,SalaryFrom,SalaryTo,Location,Description,ValidUntil,CompanyId,JobOfferStatusId,CreationDate,UserId")] JobOfferCompanyModel data)
        {
            if (id != data.JobOfferId)
            {
                return NotFound();
            }

            int userID = Helper.GetUserId(User);
            if (userID == -1 || userID != data.UserId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(data.GetJobOffer());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOffersExists(data.JobOfferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            ViewData["JobOfferStatusId"] = new SelectList(_context.JobOfferStatus, "JobOfferStatusId", "Status", data.JobOfferStatusId);
            return View(data);
        }

        //// GET: JobOffers/Delete/5
        //[Authorize(Roles = "HR,ADMIN")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var jobOffers = await _context.JobOffers
        //        .Include(j => j.Company)
        //        .Include(j => j.JobOfferStatus)
        //        .FirstOrDefaultAsync(m => m.JobOfferId == id);
        //    if (jobOffers == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(jobOffers);
        //}

        // POST: JobOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR,ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffers = await _context.JobOffers.FindAsync(id);
            if (jobOffers == null)
            {
                return NotFound();
            }
            int userId = Helper.GetUserId(User);
            if (User.IsInRole(UserRolesTypes.HR) == true)
            {
                if (jobOffers.UserId != userId)
                    return Unauthorized();
            }

            var offer = _context.JobOffers.Where(x => x.JobOfferId == id).Include(x => x.Applications);
            //TODO:delete cvs and maybe attachments

            _context.JobOffers.RemoveRange(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOffersExists(int id)
        {
            return _context.JobOffers.Any(e => e.JobOfferId == id);
        }
    }
}
