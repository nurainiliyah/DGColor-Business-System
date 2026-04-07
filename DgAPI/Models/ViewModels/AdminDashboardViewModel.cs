using System;
using System.Collections.Generic;
using DgAPI.Models;

namespace DgAPI.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalPackages { get; set; }
        public int BookingsToday { get; set; }
        public List<StudioBooking> RecentBookings { get; set; }

        // For charts
        public List<string> WeeklyLabels { get; set; }
        public List<int> WeeklyData { get; set; }
    }
}