using Microsoft.EntityFrameworkCore;
using DgAPI.Models;

namespace DgAPI.Data
{
    public class DgContext : DbContext
    {
        public DgContext(DbContextOptions<DgContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<StudioPackage> StudioPackages { get; set; }
        public DbSet<StudioBooking> StudioBookings { get; set; }
        public DbSet<PrintingService> PrintingServices { get; set; }
        public DbSet<PrintingOrder> PrintingOrders { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
