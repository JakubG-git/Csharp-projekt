using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.Data;
using TMS.Models;

namespace TMS.Controllers
{
    [Authorize]
    public class DescriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DescriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Description
        public async Task<IActionResult> Index()
        {
              return _context.Description != null ? 
                          View(await _context.Description.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Description'  is null.");
        }

        // GET: Description/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Description == null)
            {
                return NotFound();
            }

            var description = await _context.Description
                .FirstOrDefaultAsync(m => m.Id == id);
            if (description == null)
            {
                return NotFound();
            }

            return View(description);
        }

        // GET: Description/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Description/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Opis")] Description description)
        {
            if (ModelState.IsValid)
            {
                _context.Add(description);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(description);
        }

        // GET: Description/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Description == null)
            {
                return NotFound();
            }

            var description = await _context.Description.FindAsync(id);
            if (description == null)
            {
                return NotFound();
            }
            return View(description);
        }

        // POST: Description/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Opis")] Description description)
        {
            if (id != description.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(description);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescriptionExists(description.Id))
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
            return View(description);
        }

        // GET: Description/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Description == null)
            {
                return NotFound();
            }

            var description = await _context.Description
                .FirstOrDefaultAsync(m => m.Id == id);
            if (description == null)
            {
                return NotFound();
            }

            return View(description);
        }

        // POST: Description/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Description == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Description'  is null.");
            }
            var description = await _context.Description.FindAsync(id);
            if (description != null)
            {
                _context.Description.Remove(description);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DescriptionExists(int id)
        {
          return (_context.Description?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
