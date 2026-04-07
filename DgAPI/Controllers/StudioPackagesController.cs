using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DgAPI.Data;
using DgAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DgAPI.Controllers
{
    [Route("[controller]")]
    public class StudioPackagesController : Controller
    {
        private readonly DgContext _context;

        public StudioPackagesController(DgContext context)
        {
            _context = context;
        }

        // GET: StudioPackages
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var studioPackage = await _context.StudioPackages.ToListAsync();
            return View(studioPackage); // Returns the list view of packages
        }

        // GET: StudioPackages/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var studioPackage = await _context.StudioPackages.FindAsync(id);
            if (studioPackage == null)
            {
                return NotFound();
            }

            return View(studioPackage); // Returns the details view of a specific packages
        }

        // GET: StudioPackages/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(); // Returns the view for creating a new package
        }

        // POST: StudioPackages/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudioPackage studioPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studioPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the package list
            }
            return View(studioPackage);
        }

        // GET: StudioPackages/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var studioPackage = await _context.StudioPackages.FindAsync(id);
            if (studioPackage == null)
            {
                return NotFound();
            }
            return View(studioPackage); // Returns the edit view for a specific package
        }

        // POST: StudioPackages/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudioPackage studioPackage)
        {
            if (id != studioPackage.package_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studioPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudioPackageExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the package list
            }
            return View(studioPackage);
        }

        // GET: StudioPackages/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var studioPackage = await _context.StudioPackages.FindAsync(id);
            if (studioPackage == null)
            {
                return NotFound();
            }

            return View(studioPackage); // Returns the delete confirmation view
        }

        // POST: StudioPackages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studioPackage = await _context.StudioPackages.FindAsync(id);
            if (studioPackage != null)
            {
                _context.StudioPackages.Remove(studioPackage);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the package list
        }

        private bool StudioPackageExists(int id)
        {
            return _context.StudioPackages.Any(e => e.package_id == id);
        }
    }
}

