using System.Collections.Generic;
using System.Threading.Tasks;
using DgAPI.Data;
using DgAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgAPI.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class CustomersController : Controller
    {
        private readonly DgContext _context;

        public CustomersController(DgContext context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet]
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int pageSize = 5;

            var query = _context.Customers.AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.customer_name.Contains(search) ||
                    p.customer_contactNum.Contains(search)
                );
            }

            int totalRecords = await query.CountAsync();

            var customer = await query
                .OrderBy(p => p.customer_name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.Search = search;

            return View(customer);

        }

        // GET: Customers/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer); // Returns the details view of a specific customer
        }

        // GET: Customers/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(); // Returns the view for creating a new customer
        }

        // POST: Customers/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the customer list
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer); // Returns the edit view for a specific customer
        }

        // POST: Customers/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.customer_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the customer list
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer); // Returns the delete confirmation view
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the customer list
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.customer_id == id);
        }
    }
}
