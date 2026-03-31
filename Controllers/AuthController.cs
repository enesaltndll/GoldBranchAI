using GoldBranchAI.Data;
using GoldBranchAI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoldBranchAI.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
            SeedAdminUser();
        }

        private void SeedAdminUser()
        {
            if (!_context.Users.Any())
            {
                var admin = new AppUser
                {
                    FullName = "Enes Altındal (Admin)",
                    Email = "admin@test.com",
                    Password = "123",
                    Role = "Admin"
                };
                _context.Users.Add(admin);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Dashboard", "Task");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Dashboard", "Task");
            }

            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Dashboard", "Task");
            return View();
        }

        [HttpPost]
        public IActionResult Register(AppUser newUser)
        {
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                ViewBag.Error = "Bu e-posta adresi zaten kullanımda!";
                return View(newUser);
            }

            newUser.Role = "Gelistirici";
            newUser.CreatedAt = DateTime.Now;

            _context.Users.Add(newUser);

            // YENİ EKLENEN KISIM: Sisteme kayıt olan kişiyi anında Terminale (Log'a) düşür
            var log = new SystemLog
            {
                ActionType = "KAYIT",
                Message = $"Sisteme yeni bir Geliştirici katıldı: {newUser.FullName} ({newUser.Email})"
            };
            _context.SystemLogs.Add(log);

            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded) return RedirectToAction("Login");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null) return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                user = new AppUser
                {
                    FullName = name ?? "Google User",
                    Email = email,
                    Password = "GoogleOAuthLogin",
                    Role = "Gelistirici"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                _context.SystemLogs.Add(new SystemLog { ActionType = "GİRİŞ / KAYIT", Message = $"'{user.FullName}' Google ile sisteme katıldı." });
                await _context.SaveChangesAsync();
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Dashboard", "Task");
        }
    }
}