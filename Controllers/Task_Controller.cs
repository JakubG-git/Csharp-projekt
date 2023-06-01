using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.Data;
using TMS.Models;

namespace TMS.Controllers
{
    public class Task_Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public Task_Controller(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Task_
        public async Task<IActionResult> Index()
        {
              return _context.Task_ != null ? 
                          View(await _context.Task_.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Task_'  is null.");
        }

        // GET: Task_/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task_ == null)
            {
                return NotFound();
            }

            var task_ = await _context.Task_
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task_ == null)
            {
                return NotFound();
            }

            return View(task_);
        }
        
        private void PopulateRelatedTasksDropDownList(object selectedRelatedTasks = null)
        {
            var relatedTasksQuery = from d in _context.Task_
                orderby d.Title
                select d;
            ViewBag.RelatedTasks = new SelectList(relatedTasksQuery.AsNoTracking(), "Id", "Title", selectedRelatedTasks);
        }

        private void PopulateKlientIdDropDownList(object selectedKlientId = null)
        {
            var klientIdQuery = from d in _context.Klient
                orderby d.Id
                select d;
            ViewBag.KlientId = new SelectList(klientIdQuery.AsNoTracking(), "Id", "Nazwa", selectedKlientId);
        }
        private void PopulateUsersDropDownList(object selectedUser = null)
        {
            var usersQuery = from d in _context.Users
                orderby d.UserName
                select d;
            ViewBag.User = new SelectList(usersQuery.AsNoTracking(), "Id", "UserName", selectedUser);
        }

        // GET: Task_/Create
        public IActionResult Create()
        {
            PopulateKlientIdDropDownList();
            PopulateRelatedTasksDropDownList();
            PopulateUsersDropDownList();
            return View();
        }

        // POST: Task_/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Piority,Pracownik,Deadline")] Task_ task_)
        {
            string relatedTasks = Request.Form["RelatedTasks"].ToString();
            string userId = Request.Form["Pracownik"].ToString();
            string klientId = Request.Form["KlientId"].ToString();
            
            task_.Status = STATUS.NEW;
            task_.CreatedAt = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                if (relatedTasks != null && relatedTasks != "")
                {
                    task_.RelatedTasks = new List<Task_>();
                    foreach (var relatedTask in relatedTasks.Split(","))
                    {
                        task_.RelatedTasks.Add(_context.Task_.Find(int.Parse(relatedTask)));
                    }
                }
                if (userId != null && userId != "")
                {
                    task_.Pracownik = _userManager.FindByIdAsync(userId).Result.UserName;
                }
                if (klientId != null && klientId != "")
                {
                    task_.Klient = _context.Klient.Find(int.Parse(klientId));
                }
                _context.Add(task_);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task_);
        }

        // GET: Task_/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            PopulateKlientIdDropDownList();
            PopulateRelatedTasksDropDownList();
            PopulateUsersDropDownList();
            if (id == null || _context.Task_ == null)
            {
                return NotFound();
            }

            var task_ = await _context.Task_.FindAsync(id);
            if (task_ == null)
            {
                return NotFound();
            }
            return View(task_);
        }

        // POST: Task_/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Piority,Status,Pracownik,Deadline")] Task_ task_)
        {
            string relatedTasks = Request.Form["RelatedTasks"].ToString();
            string userId = Request.Form["UserId"].ToString();
            string klientId = Request.Form["KlientId"].ToString();
            if (id != task_.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (relatedTasks != null && relatedTasks != "")
                    {
                        task_.RelatedTasks = new List<Task_>();
                        foreach (var relatedTask in relatedTasks.Split(","))
                        {
                            task_.RelatedTasks.Add(_context.Task_.Find(int.Parse(relatedTask)));
                        }
                    }
                    if (userId != null && userId != "")
                    {
                        task_.Pracownik = _userManager.FindByIdAsync(userId).Result.UserName;
                    }
                    if (klientId != null && klientId != "")
                    {
                        task_.Klient = _context.Klient.Find(int.Parse(klientId));
                    }
                    _context.Update(task_);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Task_Exists(task_.Id))
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
            return View(task_);
        }

        // GET: Task_/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task_ == null)
            {
                return NotFound();
            }

            var task_ = await _context.Task_
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task_ == null)
            {
                return NotFound();
            }

            return View(task_);
        }

        // POST: Task_/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task_ == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Task_'  is null.");
            }
            var task_ = await _context.Task_.FindAsync(id);
            if (task_ != null)
            {
                _context.Task_.Remove(task_);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Task_Exists(int id)
        {
          return (_context.Task_?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
