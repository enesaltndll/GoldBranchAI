using GoldBranchAI.Data;
using GoldBranchAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoldBranchAI.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        private AppUser GetCurrentUser()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return RedirectToAction("Logout", "Auth", new { area = "" });

            ViewBag.UserRole = currentUser.Role;

            // 1. ADMIN - İSTİHBARAT MODU
            if (currentUser.Role == "Admin")
            {
                // Sistemdeki herkesin birbirine attığı TÜM mesajları tarihe göre çeker
                var allMessages = _context.ChatMessages
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .OrderByDescending(m => m.SentAt)
                    .ToList();

                ViewBag.AllMessages = allMessages;
                return View(new List<AppUser>()); // Admin listeyi değil, direkt mesaj tablosunu görecek
            }

            // 2. NORMAL KULLANICILAR (Şef ve Geliştirici İzolasyonu)
            List<AppUser> contacts;
            if (currentUser.Role == "Gelistirici")
            {
                // Geliştirici SADECE Proje Şefini görebilir
                contacts = _context.Users.Where(u => u.Role == "Proje Sefi").ToList();
            }
            else
            {
                // Proje Şefi SADECE Geliştiricileri görebilir
                contacts = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            }

            var urgentTasks = new Dictionary<int, string>();
            foreach (var user in contacts)
            {
                var urgentTask = _context.Tasks
                    .Where(t => t.AppUserId == user.Id && !t.IsCompleted)
                    .OrderBy(t => t.DueDate)
                    .FirstOrDefault();

                urgentTasks[user.Id] = urgentTask != null
                    ? $"Acil İş: {urgentTask.Title} (Teslim: {urgentTask.DueDate.ToString("dd MMM HH:mm")})"
                    : "Şu an aktif acil bir görevi yok.";
            }

            ViewBag.UrgentTasks = urgentTasks;
            return View(contacts);
        }

        public IActionResult Room(int id)
        {
            var currentUser = GetCurrentUser();
            // YENİ: Admin odaya giremez, o sadece üstten izler!
            if (currentUser.Role == "Admin") return RedirectToAction("Index");

            var targetUser = _context.Users.Find(id);
            if (targetUser == null) return RedirectToAction("Index");

            var messages = _context.ChatMessages
                .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == targetUser.Id) ||
                            (m.SenderId == targetUser.Id && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.SentAt)
                .ToList();

            ViewBag.TargetUser = targetUser;
            ViewBag.CurrentUserId = currentUser.Id;

            return View(messages);
        }

        [HttpPost]
        public IActionResult SendMessage(int receiverId, string messageText)
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Role == "Admin") return RedirectToAction("Index");

            if (!string.IsNullOrWhiteSpace(messageText))
            {
                var newMsg = new ChatMessage
                {
                    SenderId = currentUser.Id,
                    ReceiverId = receiverId,
                    MessageText = messageText,
                    SentAt = DateTime.Now
                };
                _context.ChatMessages.Add(newMsg);
                _context.SaveChanges();
            }

            return RedirectToAction("Room", new { id = receiverId });
        }
    }
}