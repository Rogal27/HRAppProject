using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;
using HRWebApplication.Services;

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
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize = 10)
        {
            var hRProjectDatabaseContext = _context.JobOffers.Include(j => j.Company).Include(j => j.JobOfferStatus);
            if(pageSize.HasValue==false)
            {
                pageSize = 10;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageNumber < 0)
            {
                pageNumber = 1;
            }
            return View(await PaginatedList<JobOffers>.CreateAsync(hRProjectDatabaseContext.AsNoTracking(), pageNumber ?? 1, pageSize.Value));
        }

        // GET: JobOffers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffers = await _context.JobOffers
                .Include(j => j.Company)
                .Include(j => j.JobOfferStatus)
                .FirstOrDefaultAsync(m => m.JobOfferId == id);
            if (jobOffers == null)
            {
                return NotFound();
            }

            return View(jobOffers);
        }

        // GET: JobOffers/Create
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
        public async Task<IActionResult> Create([Bind("JobOfferId,JobTitle,SalaryFrom,SalaryTo,Location,Description,ValidUntil,CompanyId")] JobOfferCompanyModel data)
        {
            if (!ModelState.IsValid)
            { 
                data.CompaniesCollection = await _context.Companies.ToListAsync();
                return View(data);
            }

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
                ValidUntil = data.ValidUntil
            };

            await _context.JobOffers.AddAsync(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobOffers/Edit/5
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", jobOffers.CompanyId);
            ViewData["JobOfferStatusId"] = new SelectList(_context.JobOfferStatus, "JobOfferStatusId", "Status", jobOffers.JobOfferStatusId);
            return View(jobOffers);
        }

        // POST: JobOffers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobOfferId,JobTitle,SalaryFrom,SalaryTo,Location,CreationDate,Description,ValidUntil,JobOfferStatusId,CompanyId")] JobOffers jobOffers)
        {
            if (id != jobOffers.JobOfferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOffers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOffersExists(jobOffers.JobOfferId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", jobOffers.CompanyId);
            ViewData["JobOfferStatusId"] = new SelectList(_context.JobOfferStatus, "JobOfferStatusId", "Status", jobOffers.JobOfferStatusId);
            return View(jobOffers);
        }

        // GET: JobOffers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffers = await _context.JobOffers
                .Include(j => j.Company)
                .Include(j => j.JobOfferStatus)
                .FirstOrDefaultAsync(m => m.JobOfferId == id);
            if (jobOffers == null)
            {
                return NotFound();
            }

            return View(jobOffers);
        }

        // POST: JobOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffers = await _context.JobOffers.FindAsync(id);
            _context.JobOffers.Remove(jobOffers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOffersExists(int id)
        {
            return _context.JobOffers.Any(e => e.JobOfferId == id);
        }
    }
}
