using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.Data;
using TMS.Models;

namespace TMS.Controllers
{
    [Authorize]
    public class Task_Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Task_Controller> _logger;

        public Task_Controller(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<Task_Controller> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Task_
        public async Task<IActionResult> Index()
        {
              var applicationDbContext = _context.Task_
                  .Include(t => t.Klient)
                  .Include(t => t.RelatedTasks)
                  .Include(t => t.Comments)
                  .Include(t => t.Description);
              return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> IndexUser()
        {
            var username = _userManager.GetUserName(this.User);
            var applicationDbContext = _context.Task_
                .Include(t => t.Klient)
                .Include(t => t.RelatedTasks)
                .Include(t => t.Comments)
                .Include(t => t.Description)
                .Where(t => t.Pracownik == username);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Task_/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null || _context.Task_ == null)
            {
                return NotFound();
            }

            var task_ = await _context.Task_
                .Include(t => t.Klient)
                .Include(t => t.RelatedTasks)
                .Include(t => t.Comments)
                .Include(t => t.Description)
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
        private void PopulateDescriptionDropDownList(object selectedDescription = null)
        {
            var descriptionQuery = from d in _context.Description
                select d;
            ViewBag.Description = new SelectList(descriptionQuery.AsNoTracking(), "Id", "Opis", selectedDescription);
        }
        private void PopulateCommentsDropDownList(object selectedComments = null)
        {
            var commentsQuery = from d in _context.Comments
                select d;
            
            ViewBag.Comments = new SelectList(commentsQuery.AsNoTracking(), "Id", "Komentarz", selectedComments);
            
        }

        // GET: Task_/Create
        public IActionResult Create()
        {
            PopulateKlientIdDropDownList();
            PopulateRelatedTasksDropDownList();
            PopulateUsersDropDownList();
            PopulateDescriptionDropDownList();
            PopulateCommentsDropDownList();
            return View();
        }

        // POST: Task_/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Piority,Pracownik,Deadline")] Task_ task_)
        {

            string userId = Request.Form["Pracownik"].ToString();
            string klientId = Request.Form["Klient"].ToString();
            var relatedTasks = Request.Form["RelatedTasks"].ToString();
            string description = Request.Form["Description"].ToString();
            string comments = Request.Form["Comments"].ToString();
            _logger.LogWarning("UserId: " + userId);
            _logger.LogWarning("KlientId: " + klientId);
            _logger.LogWarning("RelatedTasks: " + relatedTasks);
            _logger.LogWarning("Description: " + description);
            _logger.LogWarning("Comments: " + comments);
            task_.Status = STATUS.NEW;
            task_.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                Klient klient = new Klient();
                if (klientId != null && klientId != "")
                {
                    var kk = _context.Klient.Where(k => k.Id == int.Parse(klientId));
                    if (kk != null)
                    {
                        klient = kk.First();
                    }
                }
                task_.Klient = klient;
                
                if (relatedTasks != null && relatedTasks != "")
                {
                    task_.RelatedTasks = new List<Task_>();
                    foreach (var relatedTask in relatedTasks.Split(","))
                    {
                        task_.RelatedTasks.Add(_context.Task_.Find(int.Parse(relatedTask)));
                    }
                }
                if (description != null && description != "")
                {
                    task_.Description = _context.Description.Find(int.Parse(description));
                }
                
                if (comments != null && comments != "")
                {
                    task_.Comments = new List<Comments>();
                    foreach (var comment in comments.Split(","))
                    {
                        task_.Comments.Add(_context.Comments.Find(int.Parse(comment)));
                    }
                }

                if (userId != null)
                {
                    task_.Pracownik = _userManager.FindByIdAsync(userId).Result.UserName;
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
            PopulateDescriptionDropDownList();
            PopulateCommentsDropDownList();
            if (id == null || _context.Task_ == null)
            {
                return NotFound();
            }

            var task_ = await _context.Task_.Include(t => t.Klient)
                .Include(t => t.RelatedTasks)
                .Include(t => t.Comments)
                .Include(t => t.Description)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            string userId = Request.Form["Pracownik"].ToString();
            string klientId = Request.Form["KlientId"].ToString();
            string description = Request.Form["Description"].ToString();
            string comments = Request.Form["Comments"].ToString();
                        
            task_.CreatedAt = DateTime.Now;
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
                        ICollection<Task_> relatedTasksCollection = new List<Task_>();
                        foreach (var relatedTask in relatedTasks.Split(","))
                        {
                            relatedTasksCollection.Add(await _context.Task_.FindAsync(int.Parse(relatedTask)));
                        }
                        task_.RelatedTasks = relatedTasksCollection;
                    }
                    if (userId != null && userId != "")
                    {
                        task_.Pracownik =  _userManager.FindByIdAsync(userId).Result.UserName;
                    }
                    if (klientId != null && klientId != "")
                    {
                        task_.Klient = await _context.Klient.FindAsync(int.Parse(klientId));
                    }
                    if (description != null && description != "")
                    {
                        task_.Description = await _context.Description.FindAsync(int.Parse(description));
                    }
                    if (comments != null && comments != "")
                    {
                        ICollection<Comments> commentsCollection = new List<Comments>();
                        foreach (var comment in comments.Split(","))
                        {
                            commentsCollection.Add(await _context.Comments.FindAsync(int.Parse(comment)));
                        }
                        task_.Comments = commentsCollection;
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
                .Include(t => t.Klient)
                .Include(t => t.RelatedTasks)
                .Include(t => t.Comments)
                .Include(t => t.Description)
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
