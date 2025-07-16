using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }

        //DbSets for Booking and Resource
    }
}
