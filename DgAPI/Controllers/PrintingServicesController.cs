using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DgAPI.Data;
using DgAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DgAPI.Controllers
{
    [Route("[controller]")]
    public class PrintingServicesController : Controller
    {
        private readonly DgContext _context;

        public PrintingServicesController(DgContext context)
        {
            _context = context;
        }

        // GET: PrintingServices
        [HttpGet]
        public async Task<IActionResult> Index(string search, int page=1)
        {
            int pageSize = 5;

            var query = _context.PrintingServices.AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.printingservice_name.Contains(search) ||
                    p.printing_size.Contains(search) ||
                    p.printing_status.Contains(search)
                );
            }

            int totalRecords = await query.CountAsync();

            var printingServices = await query
                .OrderBy(p => p.printingservice_name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.Search = search;

            return View(printingServices);
        }

        // GET: PrintingServices/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var printingservice = await _context.PrintingServices.FindAsync(id);
            if (printingservice == null)
            {
                return NotFound();
            }

            return View(printingservice); // Returns the details view of a specific services
        }

        // GET: PrintingServices/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(); // Returns the view for creating a new services
        }

        // POST: PrintingServices/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrintingService printingservice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(printingservice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the service list
            }
            return View(printingservice);
        }

        // GET: PrintingServices/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var printingservice = await _context.PrintingServices.FindAsync(id);
            if (printingservice == null)
            {
                return NotFound();
            }
            return View(printingservice); // Returns the edit view for a specific service
        }

        // POST: PrintingServices/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrintingService printingservice)
        {
            if (id != printingservice.printingservice_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(printingservice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrintingServiceExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the service list
            }
            return View(printingservice);
        }

        // GET: PrintingServices/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var printingservice = await _context.PrintingServices.FindAsync(id);
            if (printingservice == null)
            {
                return NotFound();
            }

            return View(printingservice); // Returns the delete confirmation view
        }

        // POST: PrintingServices/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var printingservice = await _context.PrintingServices.FindAsync(id);
            if (printingservice != null)
            {
                _context.PrintingServices.Remove(printingservice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the service list
        }

        private bool PrintingServiceExists(int id)
        {
            return _context.PrintingServices.Any(e => e.printingservice_id == id);
        }
    }
}

