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
            ViewBag.CurrentUserId = currentUser.Id;

            // 1. Grupları Getir
            List<ChatGroup> activeGroups;
            if (currentUser.Role == "Admin")
            {
                activeGroups = _context.ChatGroups.Include(g => g.Members).ToList();
            }
            else
            {
                var myGroupIds = _context.ChatGroupMembers
                    .Where(m => m.AppUserId == currentUser.Id)
                    .Select(m => m.ChatGroupId)
                    .ToList();

                activeGroups = _context.ChatGroups
                    .Include(g => g.Members)
                    .Where(g => myGroupIds.Contains(g.Id))
                    .ToList();
            }
            ViewBag.Groups = activeGroups;

            // 2. Kişileri Getir
            List<AppUser> contacts = new List<AppUser>();
            if (currentUser.Role == "Admin")
            {
                // Admin hem Proje Şeflerine mesaj atabilir hem de Geliştirici-Şef konuşmalarını izleyebilir
                // Şef listesini getir (Direkt Mesaj için)
                contacts = _context.Users.Where(u => u.Role == "Proje Sefi").ToList();
                
                // İzleme modunda gösterilecek "Konuşma Çiftleri" Sidebar için özel bir yapı gerekebilir.
                // Şimdilik sadece aktif kullanıcıları gösteriyoruz, izleme modunu Room üzerinden kontrol edeceğiz.
                ViewBag.DevsForMonitoring = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            }
            else if (currentUser.Role == "Gelistirici")
            {
                contacts = _context.Users.Where(u => u.Role == "Proje Sefi").ToList();
            }
            else // Proje Şefi
            {
                contacts = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            }

            return View(contacts);
        }

        public IActionResult Room(int? id, int? groupId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return Unauthorized();

            ViewBag.CurrentUserId = currentUser.Id;
            ViewBag.UserRole = currentUser.Role;

            // GRUP SOHBETİ
            if (groupId.HasValue)
            {
                var group = _context.ChatGroups
                    .Include(g => g.Members)
                    .ThenInclude(m => m.AppUser)
                    .FirstOrDefault(g => g.Id == groupId.Value);

                if (group == null) return NotFound();

                // Admin değilse ve üye değilse erişemez
                if (currentUser.Role != "Admin" && !group.Members.Any(m => m.AppUserId == currentUser.Id))
                {
                    return Forbid();
                }

                var messages = _context.ChatMessages
                    .Include(m => m.Sender)
                    .Where(m => m.ChatGroupId == groupId.Value)
                    .OrderBy(m => m.SentAt)
                    .ToList();

                ViewBag.IsGroup = true;
                ViewBag.TargetGroup = group;
                return View(messages);
            }

            // DİREKT MESAJ (DM)
            if (id.HasValue)
            {
                var targetUser = _context.Users.Find(id.Value);
                if (targetUser == null) return NotFound();

                // Admin kısıtlamaları ve izleme modu
                // Eğer Admin bir Geliştiriciye tıklarsa, araya bir "Şef" seçmesi gerekebilir izleme için.
                // Bu basitleştirilmiş versiyonda Admin -> Şef DM, Admin -> Dev (İzleme) şeklinde kurguluyoruz.
                
                var messages = _context.ChatMessages
                    .Include(m => m.Sender)
                    .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == id.Value) ||
                                (m.SenderId == id.Value && m.ReceiverId == currentUser.Id))
                    .OrderBy(m => m.SentAt)
                    .ToList();

                ViewBag.IsGroup = false;
                ViewBag.TargetUser = targetUser;
                return View(messages);
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult SendMessage(int? receiverId, int? groupId, string messageText)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return Unauthorized();

            // Admin sadece Şeflere mesaj atabilir
            if (currentUser.Role == "Admin" && receiverId.HasValue)
            {
                var target = _context.Users.Find(receiverId.Value);
                if (target?.Role != "Proje Sefi") return Forbid();
            }

            if (!string.IsNullOrWhiteSpace(messageText))
            {
                var newMsg = new ChatMessage
                {
                    SenderId = currentUser.Id,
                    ReceiverId = receiverId,
                    ChatGroupId = groupId,
                    MessageText = messageText,
                    SentAt = DateTime.Now
                };
                _context.ChatMessages.Add(newMsg);
                _context.SaveChanges();
            }

            return RedirectToAction("Room", new { id = receiverId, groupId = groupId, iframe = true });
        }

        [HttpPost]
        public IActionResult CreateGroup(string groupName, int[] selectedUserIds)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null || (currentUser.Role != "Admin" && currentUser.Role != "Proje Sefi"))
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(groupName) || selectedUserIds == null || selectedUserIds.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var group = new ChatGroup
            {
                GroupName = groupName,
                CreatedByUserId = currentUser.Id,
                CreatedAt = DateTime.Now
            };

            _context.ChatGroups.Add(group);
            _context.SaveChanges();

            // Yaratan kişiyi ve seçilenleri ekle
            var allMembers = selectedUserIds.ToList();
            if (!allMembers.Contains(currentUser.Id)) allMembers.Add(currentUser.Id);

            foreach (var userId in allMembers)
            {
                _context.ChatGroupMembers.Add(new ChatGroupMember
                {
                    ChatGroupId = group.Id,
                    AppUserId = userId
                });
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}