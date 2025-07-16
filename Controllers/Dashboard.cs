using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using InternalResourceBookingSystem.Data;

namespace InternalResourceBookingSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalResources = await _context.Resources.CountAsync();
            var totalBookings = await _context.Bookings.CountAsync();

            var upcomingBookings = await _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.StartTime > DateTime.Now)
                .OrderBy(b => b.StartTime)
                .Take(3)
                .ToListAsync();

            var todaysBookings = await _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.StartTime.Date == DateTime.Today)
                .ToListAsync();

            ViewBag.TotalResources = totalResources;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.UpcomingBookings = upcomingBookings;
            ViewBag.TodaysBookings = todaysBookings;

            return View();
        }
    }
}

