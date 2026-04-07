using System.Security.Claims;
using DgAPI.Data;
using DgAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace DgAPI.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly DgContext _context;

        public AuthController(DgContext context)
        {
            _context = context;
        }
            // GET: Auth/Login
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            // POST: Auth/Login
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(string username, string password)
            {
                Console.WriteLine($"Login attempt for username: {username}");

                var user = await _context.Users.FirstOrDefaultAsync(s => s.username == username);

                if (user == null)
                {
                    Console.WriteLine("User not found in database");
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }

                Console.WriteLine($"Found user: {user.username}, Stored Hash: {user.password}");

                if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    Console.WriteLine("Password verification failed");
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }

                Console.WriteLine("Password verification passed, logging in...");

                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.username),
        new Claim("user_id", user.user_id.ToString()),
        new Claim(ClaimTypes.Role, user.role)
    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            if (user.role == "Admin")
            {
                return RedirectToAction("Index", "AdminDashboard");   // Admin dashboard
            }
            else
            {
                return RedirectToAction("Index", "StaffDashboard"); // Staff landing page
            }
            
            }

            // POST: Auth/Register
            [HttpPost]
            public async Task<IActionResult> Register(string user_fullname, string username, string password, string email)
            {
                if (await _context.Users.AnyAsync(s => s.username == username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View();
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var user = new User
                {
                    user_fullname = user_fullname,
                    username = username,
                    password = hashedPassword,
                    email = email,
                    role = "Staff"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            // POST: Auth/Logout
            [HttpPost]
            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }
        }
    }