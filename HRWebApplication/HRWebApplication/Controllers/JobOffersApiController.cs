using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;
using HRWebApplication.Services;
using HRWebApplication.Helpers;

namespace HRWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOffersApiController : ControllerBase
    {
        private readonly HRProjectDatabaseContext _context;

        public JobOffersApiController(HRProjectDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/JobOffersApi
        [HttpGet]
        public async Task<PagingViewModel<JobOffers>> GetJobOffers(int pageNumber = 1, int pageSize = 10, string searchString = "", int? companyId = null, string location = null, int? salaryFrom = 0, int? salaryTo = int.MaxValue, string sortOrder = null)
        {
            var result = _context.JobOffers.Include(j => j.Company).Include(j => j.JobOfferStatus);
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (salaryFrom.HasValue == false)
            {
                salaryFrom = 0;
            }
            if(salaryTo.HasValue == false)
            {
                salaryTo = int.MaxValue;
            }
            if (searchString == null)
            {
                searchString = "";
            }
            if (salaryFrom < 0)
            {
                salaryFrom = 0;
            }
            if (salaryTo < salaryFrom)
            {
                salaryTo = salaryFrom;
            }
            if(sortOrder == null)
            {
                sortOrder = "";
            }

            var result_filter = result
                .Where(p => p.JobTitle.ToLower().Contains(searchString.ToLower()))
                .Where(p => p.SalaryFrom.HasValue == false || p.SalaryFrom.Value >= salaryFrom)
                .Where(p => p.SalaryTo.HasValue == false || p.SalaryTo.Value <= salaryTo);

            if (location != null && companyId != null)
            {
                result_filter = result_filter.Where(p => p.Location == location).Where(p => p.CompanyId == companyId);
            }
            else if (location != null)
            {
                result_filter = result_filter.Where(p => p.Location == location);
            }
            else if (companyId != null)
            {
                result_filter = result_filter.Where(p => p.CompanyId == companyId);
            }       
            
            switch(sortOrder)
            {
                case "title_desc":
                    result_filter = result_filter.OrderByDescending(j => j.JobTitle).ThenByDescending(j => j.CreationDate);
                    break;
                case "title_asc":
                    result_filter = result_filter.OrderBy(j => j.JobTitle).ThenByDescending(j => j.CreationDate);
                    break;
                case "company_desc":
                    result_filter = result_filter.OrderByDescending(j => j.Company.CompanyName).ThenByDescending(j => j.CreationDate);
                    break;
                case "company_asc":
                    result_filter = result_filter.OrderBy(j => j.Company.CompanyName).ThenByDescending(j => j.CreationDate);
                    break;
                case "location_desc":
                    result_filter = result_filter.OrderByDescending(j => j.Location).ThenByDescending(j => j.CreationDate);
                    break;
                case "location_asc":
                    result_filter = result_filter.OrderBy(j => j.Location).ThenByDescending(j => j.CreationDate);
                    break;
                case "salary_desc":
                    result_filter = result_filter.OrderByDescending(j => j.SalaryFrom).ThenByDescending(j => j.CreationDate);
                    break;
                case "salary_asc":
                    result_filter = result_filter.OrderBy(j => j.SalaryFrom).ThenByDescending(j => j.CreationDate);
                    break;
                case "date_asc":
                    result_filter = result_filter.OrderBy(j => j.CreationDate);
                    break;
                default:
                case "date_desc":
                    result_filter = result_filter.OrderByDescending(j => j.CreationDate);
                    break;
            }

            int totalRecord = await result_filter.CountAsync();
            var record = await result_filter.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            PagingViewModel<JobOffers> data = new PagingViewModel<JobOffers>(record, totalRecord, pageSize);
            return data;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<PagingViewModel<JobOffers>> GetJobOffersForHR(int pageNumber = 1, int pageSize = 10, string searchString = "", int? companyId = null, string location = null, int? salaryFrom = 0, int? salaryTo = int.MaxValue, string sortOrder = null, int? HRID = -1)
        {
            Users HRUser = null;
            if (HRID.HasValue == true)
            {
                HRUser = await _context.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.UserId == HRID.Value && u.UserRole.Role == UserRolesTypes.HR);
            }
            if (HRUser == null)
            {
                return null;
            }

            var result = _context.JobOffers.Include(j => j.Company).Include(j => j.JobOfferStatus).Where(j => j.UserId == HRID);

            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (salaryFrom.HasValue == false)
            {
                salaryFrom = 0;
            }
            if (salaryTo.HasValue == false)
            {
                salaryTo = int.MaxValue;
            }
            if (searchString == null)
            {
                searchString = "";
            }
            if (salaryFrom < 0)
            {
                salaryFrom = 0;
            }
            if (salaryTo < salaryFrom)
            {
                salaryTo = salaryFrom;
            }
            if (sortOrder == null)
            {
                sortOrder = "";
            }

            var result_filter = result
                .Where(p => p.JobTitle.ToLower().Contains(searchString.ToLower()))
                .Where(p => p.SalaryFrom.HasValue == false || p.SalaryFrom.Value >= salaryFrom)
                .Where(p => p.SalaryTo.HasValue == false || p.SalaryTo.Value <= salaryTo);

            if (location != null && companyId != null)
            {
                result_filter = result_filter.Where(p => p.Location == location).Where(p => p.CompanyId == companyId);
            }
            else if (location != null)
            {
                result_filter = result_filter.Where(p => p.Location == location);
            }
            else if (companyId != null)
            {
                result_filter = result_filter.Where(p => p.CompanyId == companyId);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    result_filter = result_filter.OrderByDescending(j => j.JobTitle).ThenByDescending(j => j.CreationDate);
                    break;
                case "title_asc":
                    result_filter = result_filter.OrderBy(j => j.JobTitle).ThenByDescending(j => j.CreationDate);
                    break;
                case "company_desc":
                    result_filter = result_filter.OrderByDescending(j => j.Company.CompanyName).ThenByDescending(j => j.CreationDate);
                    break;
                case "company_asc":
                    result_filter = result_filter.OrderBy(j => j.Company.CompanyName).ThenByDescending(j => j.CreationDate);
                    break;
                case "location_desc":
                    result_filter = result_filter.OrderByDescending(j => j.Location).ThenByDescending(j => j.CreationDate);
                    break;
                case "location_asc":
                    result_filter = result_filter.OrderBy(j => j.Location).ThenByDescending(j => j.CreationDate);
                    break;
                case "salary_desc":
                    result_filter = result_filter.OrderByDescending(j => j.SalaryFrom).ThenByDescending(j => j.CreationDate);
                    break;
                case "salary_asc":
                    result_filter = result_filter.OrderBy(j => j.SalaryFrom).ThenByDescending(j => j.CreationDate);
                    break;
                case "date_asc":
                    result_filter = result_filter.OrderBy(j => j.CreationDate);
                    break;
                default:
                case "date_desc":
                    result_filter = result_filter.OrderByDescending(j => j.CreationDate);
                    break;
            }

            int totalRecord = await result_filter.CountAsync();
            var record = await result_filter.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            PagingViewModel<JobOffers> data = new PagingViewModel<JobOffers>(record, totalRecord, pageSize);
            return data;
        }

        // GET: api/JobOffersApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobOffers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobOffers = await _context.JobOffers.FindAsync(id);

            if (jobOffers == null)
            {
                return NotFound();
            }

            return Ok(jobOffers);
        }

        // PUT: api/JobOffersApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobOffers([FromRoute] int id, [FromBody] JobOffers jobOffers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobOffers.JobOfferId)
            {
                return BadRequest();
            }

            _context.Entry(jobOffers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobOffersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobOffersApi
        [HttpPost]
        public async Task<IActionResult> PostJobOffers([FromBody] JobOffers jobOffers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.JobOffers.Add(jobOffers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobOffers", new { id = jobOffers.JobOfferId }, jobOffers);
        }

        // DELETE: api/JobOffersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobOffers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobOffers = await _context.JobOffers.FindAsync(id);
            if (jobOffers == null)
            {
                return NotFound();
            }

            _context.JobOffers.Remove(jobOffers);
            await _context.SaveChangesAsync();

            return Ok(jobOffers);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<string>> GetLocations()
        {
            var data = await _context.JobOffers.Select(s => s.Location).Distinct().OrderBy(s => s).ToListAsync();
            return data;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<Companies>> GetCompanies()
        {
            var data = await _context.Companies.OrderBy(c => c.CompanyName).ToListAsync();
            return data;
        }

        private bool JobOffersExists(int id)
        {
            return _context.JobOffers.Any(e => e.JobOfferId == id);
        }
    }
}