﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString, DateTime? date)
        {
            var bookings = _context.Bookings.Include(b => b.Resource).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => b.Resource.Name.Contains(searchString));
            }

            if (date.HasValue)
            {
                bookings = bookings.Where(b => b.StartTime.Date == date.Value.Date);
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.ResourceList = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            // Validate times
            if (booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError("", "End time must be after start time.");
            }

            // Booking conflict check
            var hasConflict = _context.Bookings.Any(b =>
                b.ResourceId == booking.ResourceId &&
                (
                    (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                    (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                    (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
                ));

            if (hasConflict)
            {
                ModelState.AddModelError("", "This resource is already booked during the selected time.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = booking.Id });
            }

            ViewBag.ResourceList = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var resources = _context.Resources
                .Where(r => r.IsAvailable || r.Id == booking.ResourceId)
                .ToList();

            ViewBag.ResourceId = new SelectList(resources, "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check for time conflict with other bookings (excluding itself)
                var conflict = await _context.Bookings
                    .Where(b => b.ResourceId == booking.ResourceId && b.Id != booking.Id)
                    .Where(b => booking.StartTime < b.EndTime && booking.EndTime > b.StartTime)
                    .FirstOrDefaultAsync();

                if (conflict != null)
                {
                    ModelState.AddModelError(string.Empty, "This resource is already booked during the selected time.");
                    ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
                    return View(booking);
                }

                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
