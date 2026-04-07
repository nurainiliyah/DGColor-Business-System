using System.Collections.Generic;
using System.Threading.Tasks;
using DgAPI.Data;
using DgAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DgAPI.Controllers
{
    [Route("[controller]")]
    public class PrintingOrdersController : Controller
    {
        private readonly DgContext _context;

        public PrintingOrdersController(DgContext context)
        {
            _context = context;
        }

        // GET: PrintingOrders
        [HttpGet]
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int pageSize = 5;

            var query = _context.PrintingOrders
            .Include(p => p.customers)
            .Include(p => p.printingServices)
            .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.customers.customer_name.Contains(search) ||
                    p.printingServices.printingservice_name.Contains(search)
                );
            }

            int totalRecords = await query.CountAsync();

            var printingOrders = await query
            .OrderByDescending(p => p.print_orderdate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.Search = search;

            return View(printingOrders);
        }

        // GET: PrintingOrders/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var printingOrder = await _context.PrintingOrders.FindAsync(id);
            if (printingOrder == null)
            {
                return NotFound();
            }

            return View(printingOrder); // Returns the details view of a specific orders
        }
        [HttpGet("SearchCustomer")]
        public async Task<IActionResult> SearchCustomer(string term)
        {
            var customers = await _context.Customers
                .Where(c => c.customer_name.Contains(term))
                .Select(c => new
                {
                    id = c.customer_id,
                    name = c.customer_name
                })
                .Take(10)
                .ToListAsync();

            return Json(customers);
        }

        // GET: PrintingOrders/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create(string searchCustomer)
        {
            // Load packages (need this for package_id)
            ViewBag.ServiceList = new SelectList(
                _context.PrintingServices
                .Where(s => s.printing_status == "Active"),
                "printingservice_id",
                "printingservice_name"
            );
            ViewBag.ServicePrices = await _context.PrintingServices
        .ToDictionaryAsync(s => s.printingservice_id, s => s.printing_price);

            // Search customer if search text exists
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                ViewBag.CustomerResults = await _context.Customers
                    .Where(c => c.customer_name.Contains(searchCustomer))
                    .ToListAsync();
            }

            return View();
        }

        // POST: PrintingOrders/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrintingOrder printingOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(printingOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the order list
            }
            ViewBag.ServiceList = new SelectList(
        _context.PrintingServices,
        "printingservice_id",
        "printingservice_name");
            return View(printingOrder);
        }

        // GET: PrintingOrders/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var printingOrder = await _context.PrintingOrders
            .Include(b => b.customers)
            .FirstOrDefaultAsync(b => b.printingorder_id == id); 
            if (printingOrder == null)
            {
                return NotFound();
            }
            ViewBag.ServiceList = new SelectList(
                _context.PrintingServices 
                .Where(s => s.printing_status == "Active"),
                "printingservice_id",
                "printingservice_name",
                printingOrder.printingservice_id
            );
            ViewBag.ServicePrices = await _context.PrintingServices
       .ToDictionaryAsync(s => s.printingservice_id, s => s.printing_price);

            return View(printingOrder); // Returns the edit view for a specific order

        }

        // POST: PrintingOrders/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrintingOrder printingOrder)
        {
            if (id != printingOrder.printingorder_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(printingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrintingOrderExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the order list
            }
            return View(printingOrder);
        }

        // GET: PrintingOrders/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var printingOrder = await _context.PrintingOrders.FindAsync(id);
            if (printingOrder == null)
            {
                return NotFound();
            }

            return View(printingOrder); // Returns the delete confirmation view
        }

        // POST: PrintingOrders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var printingOrder = await _context.PrintingOrders.FindAsync(id);
            if (printingOrder != null)
            {
                _context.PrintingOrders.Remove(printingOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the order list
        }

        private bool PrintingOrderExists(int id)
        {
            return _context.PrintingOrders.Any(e => e.printingorder_id == id);
        }
    }
}

