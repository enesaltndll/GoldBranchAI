using GoldBranchAI.Data;
using GoldBranchAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoldBranchAI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Admin";

        public IActionResult Users()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");
            return View(_context.Users.OrderBy(u => u.Role).ToList());
        }

        [HttpPost]
        public IActionResult ChangeRole(int userId, string newRole)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");
            var user = _context.Users.Find(userId);
            if (user != null && user.Role != "Admin")
            {
                user.Role = newRole;
                _context.SystemLogs.Add(new SystemLog { ActionType = "SİSTEM", Message = $"{user.FullName} adlı kişinin rolü '{newRole}' olarak değiştirildi." });
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        public IActionResult WorkReports()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");
            var logs = _context.DailyTimeLogs.Include(l => l.AppUser).Where(l => l.LogDate >= DateTime.Today.AddDays(-7)).OrderByDescending(l => l.LogDate).ToList();
            return View(logs);
        }

        // 1. Tükenmişlik (Burnout) Isı Haritası (ÇÖKME HATASI DÜZELTİLDİ)
        public IActionResult BurnoutMap()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            var rawLogs = _context.DailyTimeLogs
                .Include(l => l.AppUser)
                .Where(l => l.LogDate >= startOfWeek && l.AppUser.Role != "Admin")
                .ToList();

            // ViewBag'in çökmemesi için verileri güvenli Tuple yapısına dönüştürüyoruz
            var burnoutData = rawLogs
                .GroupBy(l => l.AppUser)
                .Select(g => new Tuple<AppUser, int>(g.Key, g.Sum(x => x.TotalMinutes)))
                .OrderByDescending(x => x.Item2)
                .ToList();

            ViewBag.BurnoutData = burnoutData;
            return View();
        }

        public IActionResult SystemLogs()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");

            if (!_context.SystemLogs.Any())
            {
                _context.SystemLogs.Add(new SystemLog { ActionType = "GÜVENLİK", Message = "Sistem duvarı (Firewall) başarıyla başlatıldı.", CreatedAt = DateTime.Now.AddMinutes(-50) });
                _context.SystemLogs.Add(new SystemLog { ActionType = "VERİTABANI", Message = "GoldBranch AI çekirdek bağlantısı sağlandı.", CreatedAt = DateTime.Now.AddMinutes(-45) });
                _context.SaveChanges();
            }

            var logs = _context.SystemLogs.OrderByDescending(l => l.CreatedAt).Take(50).ToList();
            return View(logs);
        }

        public IActionResult Leaderboard()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Task");

            var topDevs = _context.Users
                .Where(u => u.Role == "Gelistirici")
                .Include(u => u.TodoTasks)
                .Select(u => new
                {
                    User = u,
                    CompletedCount = u.TodoTasks.Count(t => t.IsCompleted),
                    ActiveCount = u.TodoTasks.Count(t => !t.IsCompleted)
                })
                .OrderByDescending(x => x.CompletedCount)
                .ToList();

            ViewBag.TopDevs = topDevs;
            return View();
        }
    }
}