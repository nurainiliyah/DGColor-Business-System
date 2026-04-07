using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DgAPI.Data;
using DgAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DgAPI.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffDashboardController : Controller
    {
        private readonly DgContext _context;
        public StaffDashboardController(DgContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var todayDateTime = DateTime.Today;

            // Bookings Today
            var bookingsToday = await _context.StudioBookings
                .Where(b => b.booking_date == todayDateOnly)
                .CountAsync();

            // Printing Orders Today
            var printingToday = await _context.PrintingOrders
                .Where(p => p.print_orderdate.Date == DateTime.Today)
                .CountAsync();

            // Send to View
            ViewBag.BookingsToday = bookingsToday;
            ViewBag.PrintingToday = printingToday;

            return View(); // Views/StaffDashboard/Index.cshtml
        }
    }
}