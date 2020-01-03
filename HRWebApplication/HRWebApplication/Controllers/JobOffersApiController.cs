using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRWebApplication.Models;
using HRWebApplication.Services;

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
        public async Task<PagingViewModel<JobOffers>> GetJobOffers(int? pageNumber = 1, int? pageSize = 10)
        {
            //return await _context.JobOffers.Include(j => j.JobOfferStatus).ToListAsync();

            var hRProjectDatabaseContext = _context.JobOffers.Include(j => j.Company).Include(j => j.JobOfferStatus).OrderByDescending(j => j.CreationDate);
            if (pageSize.HasValue == false)
            {
                pageSize = 10;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int totalPage, totalRecord;

            totalRecord = await hRProjectDatabaseContext.CountAsync();
            //totalPage = (totalRecord / pageSize.Value) + ((totalRecord % pageSize.Value) > 0 ? 1 : 0);
            var record = await hRProjectDatabaseContext.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();

            PagingViewModel<JobOffers> data = new PagingViewModel<JobOffers>(record, totalRecord, pageSize.Value);
            return data;
            //return await PaginatedList<JobOffers>.CreateAsync(hRProjectDatabaseContext.AsNoTracking(), pageNumber ?? 1, pageSize.Value);
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

        private bool JobOffersExists(int id)
        {
            return _context.JobOffers.Any(e => e.JobOfferId == id);
        }
    }
}