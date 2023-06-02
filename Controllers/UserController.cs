using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.Data;
using TMS.Models;

namespace TMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        // GET: User
        public async Task<IActionResult> Index()    
        {
            return _context.Users != null ? 
                View(await _context.Users.ToListAsync()) :
                Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }
        
        private void PopulateRole(string userId)
        {
            if (_context.Roles == null)
            {
                throw new Exception("Entity set 'ApplicationDbContext.Roles'  is null.");
            }
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();
            var userRoleNames = new List<string>();
            foreach (var userRole in userRoles)
            {
                var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                if (role != null)
                {
                    userRoleNames.Add(role.Name);
                }
            }
            ViewBag.Roles = userRoleNames;
        }
        private void PopulateRolesDropDownList()
        {
            if (_context.Roles == null)
            {
                throw new Exception("Entity set 'ApplicationDbContext.Roles'  is null.");
            }
            var rolesQuery = from r in _context.Roles
                orderby r.Name
                select r;
            ViewBag.RolesDrop = new SelectList(rolesQuery.AsNoTracking(), "Name", "Name");
        }
        
        public IActionResult Create()
        {
            PopulateRolesDropDownList();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Email, Password, Role")] RegisterModelRole user)
        {
            _logger.LogDebug("Password: " + user.Password);
            _logger.LogDebug("Role: " + user.Role);
            _logger.LogDebug("Email: " + user.Email);
            if (ModelState.IsValid)
            {
                var newUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                //print user.Input.Password
                

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, user.Role);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(HomeController.Index),
                nameof(HomeController).Replace("Controller", string.Empty));
        }
        
        // GET: User/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            PopulateRole(id);
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        
        // GET: User/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            PopulateRole(id);
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: User/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            PopulateRolesDropDownList();
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Klient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Username,Email")] IdentityUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
    
}

