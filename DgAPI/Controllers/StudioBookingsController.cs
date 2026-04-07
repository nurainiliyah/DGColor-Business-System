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
    public class StudioBookingsController : Controller
    {
        private readonly DgContext _context;

        public StudioBookingsController(DgContext context)
        {
            _context = context;
        }

        // GET: StudioBookings
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var studioBooking = await _context.StudioBookings.ToListAsync();
            return View(studioBooking); // Returns the list view of bookings
        }

        // GET: StudioBookings/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var studioBooking = await _context.StudioBookings.FindAsync(id);
            if (studioBooking == null)
            {
                return NotFound();
            }

            return View(studioBooking); // Returns the details view of a specific bookings
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

        [HttpGet("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability(string date, string time, int? bookingId)
        {
            var bookingDate = DateOnly.Parse(date);
            var bookingTime = TimeOnly.Parse(time);

            bool exists = await _context.StudioBookings.AnyAsync(b =>
            b.booking_date == bookingDate &&
            b.booking_time == bookingTime &&
            b.booking_id != bookingId
            );

            return Json(new { available = !exists });
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.StudioBookings
                .Include(b => b.customers)
                .Include(b => b.studioPackages)
                .ToListAsync();

            var events = bookings.Select(b => new {
                id = b.booking_id,
                title = $"{b.customers.customer_name} - {b.studioPackages.package_name}",
                start = b.booking_date.ToDateTime(b.booking_time), // convert DateOnly + TimeOnly
                end = b.booking_date.ToDateTime(b.booking_time.AddHours(1)), // 1 hour slot
                color = "#28a745"
            });

            return Json(events);
        }
        // GET: StudioBookings/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create(string searchCustomer, DateOnly? date)
        {
            // Load packages (you will need this for package_id)
            ViewBag.PackageList = new SelectList(
                _context.StudioPackages
                .Where(s => s.package_status == "Active"),
                "package_id",
                "package_name"
            );

            // Search customer if search text exists
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                ViewBag.CustomerResults = await _context.Customers
                    .Where(c => c.customer_name.Contains(searchCustomer))
                    .ToListAsync();
            }
            
                if (date.HasValue)
                {
                    ViewBag.SelectedDate = date.Value;
                }
            return View();
        }
        // POST: StudioBookings/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudioBooking studioBooking)
        {
            // Check if the selected slot is already taken
            bool exists = await _context.StudioBookings.AnyAsync(b =>
                b.booking_date == studioBooking.booking_date &&
                b.booking_time == studioBooking.booking_time
            );

            if (exists)
            {
                ModelState.AddModelError("booking_time", "This time slot is already booked. Please choose another.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(studioBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the bookings list
            }
            ViewBag.PackageList = new SelectList(
        _context.StudioPackages,
        "package_id",
        "package_name"
    );
            return View(studioBooking);
        }

        // GET: StudioBookings/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var studioBooking = await _context.StudioBookings
            .Include(b => b.customers)
            .FirstOrDefaultAsync(b => b.booking_id == id);
            if (studioBooking == null)
            {
                return NotFound();
            }
            // Load package list for the dropdown
            ViewBag.PackageList = new SelectList(
                _context.StudioPackages
                .Where(s => s.package_status == "Active"),
                "package_id",
                "package_name"
            );
            return View(studioBooking); // Returns the edit view for a specific booking
        }

        // POST: StudioBookings/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudioBooking studioBooking)
        {
            if (id != studioBooking.booking_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studioBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudioBookingExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the booking list
            }
            return View(studioBooking);
        }

        // GET: StudioBookings/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var studioBooking = await _context.StudioBookings.FindAsync(id);
            if (studioBooking == null)
            {
                return NotFound();
            }

            return View(studioBooking); // Returns the delete confirmation view
        }

        // POST: StudioBookings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int booking_id)
        {
            var studioBooking = await _context.StudioBookings.FindAsync(booking_id);
            if (studioBooking != null)
            {
                _context.StudioBookings.Remove(studioBooking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the booking list
        }

        private bool StudioBookingExists(int id)
        {
            return _context.StudioBookings.Any(e => e.booking_id == id);
        }
    }
}
