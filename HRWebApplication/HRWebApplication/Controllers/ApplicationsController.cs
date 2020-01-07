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
        //public async Task<IActionResult> Index()
        //{
        //    var hRProjectDatabaseContext = _context.Applications.Include(a => a.ApplicationStatus).Include(a => a.Cv).Include(a => a.JobOffer).Include(a => a.User);
        //    return View(await hRProjectDatabaseContext.ToListAsync());
        //}

        

        

        
    }
}
