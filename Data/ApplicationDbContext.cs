using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }
        //Tables for resources and bookings
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
