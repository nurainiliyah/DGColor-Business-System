using System;
using System.Linq;
using System.Threading.Tasks;
using DgAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminDashboardController : Controller
    {
        private readonly DgContext _context;

        public AdminDashboardController(DgContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // TOTAL REVENUE
            var bookingRevenue = await _context.StudioBookings
                .Include(b => b.studioPackages)
                .SumAsync(b => (decimal?)b.studioPackages.package_price) ?? 0;

            var printingRevenue = await _context.PrintingOrders
                .SumAsync(p => (decimal?)p.print_totalprice) ?? 0;

            ViewBag.TotalRevenue = bookingRevenue + printingRevenue;
            ViewBag.BookingRevenue = bookingRevenue;
            ViewBag.PrintingRevenue = printingRevenue;

            // TOTAL SALES TODAY
            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var todayDateTime = DateTime.Today;

            var bookingSalesToday = await _context.StudioBookings
                .Where(b => b.booking_date == todayDateOnly)
                .Include(b => b.studioPackages)
                .SumAsync(b => (decimal?)b.studioPackages.package_price) ?? 0;

            var printingSalesToday = await _context.PrintingOrders
                .Where(p => p.print_orderdate.Date == todayDateTime)
                .SumAsync(p => (decimal?)p.print_totalprice) ?? 0;

            ViewBag.TotalSalesToday = bookingSalesToday + printingSalesToday;

            // BOOKINGS PER PACKAGE PIE
            var packageStats = await _context.StudioBookings
                .Include(b => b.studioPackages)
                .GroupBy(b => b.studioPackages.package_name)
                .Select(g => new { Package = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.PackageLabels = packageStats.Select(x => x.Package).ToList();
            ViewBag.PackageCounts = packageStats.Select(x => x.Count).ToList();

            // PRINTING PER SERVICE PIE
            var printingServiceStats = await _context.PrintingOrders
                .Include(p => p.printingServices)
                .GroupBy(p => p.printingServices.printingservice_name)
                .Select(g => new { Service = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.PrintingServiceLabels = printingServiceStats.Select(x => x.Service).ToList();
            ViewBag.PrintingServiceCounts = printingServiceStats.Select(x => x.Count).ToList();

            // LAST 6 MONTHS REVENUE (Booking + Printing)
            var sixMonthsAgo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-5);

            var monthList = Enumerable.Range(0, 6)
                .Select(i => sixMonthsAgo.AddMonths(i))
                .ToList();

            var monthlyBookingCounts = monthList
                .Select(m => _context.StudioBookings
                    .Include(b => b.studioPackages)
                    .Where(b => b.booking_date.Year == m.Year && b.booking_date.Month == m.Month)
                    .Sum(b => (decimal?)b.studioPackages.package_price) ?? 0)
                .ToList();

            var monthlyPrintingCounts = monthList
                .Select(m => _context.PrintingOrders
                    .Where(p => p.print_orderdate.Year == m.Year && p.print_orderdate.Month == m.Month)
                    .Sum(p => (decimal?)p.print_totalprice) ?? 0)
                .ToList();

            ViewBag.MonthlyLabels = monthList.Select(m => m.ToString("MMM yyyy")).ToList();
            ViewBag.MonthlyBookingCounts = monthlyBookingCounts;
            ViewBag.MonthlyPrintingCounts = monthlyPrintingCounts;

            return View();
        }
    }
}