using System.Collections.Generic;
using System.Threading.Tasks;
using DgAPI.Data;
using DgAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly DgContext _context;

        public UsersController(DgContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.ToListAsync();
            return View(user); // Returns the list view of users
        }

        // GET: Users/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Returns the details view of a specific user
        }

        // GET: Users/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(); // Returns the view for creating a new user
        }

        // POST: Users/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirects to the user list
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Returns the edit view for a specific user
        }

        // POST: Users/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.user_id)
            {
                return BadRequest();
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
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redirects to the user list
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Returns the delete confirmation view
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirects to the user list
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.user_id == id);
        }
    }
}
