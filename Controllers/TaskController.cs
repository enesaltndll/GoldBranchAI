using GoldBranchAI.Data;
using GoldBranchAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoldBranchAI.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaskController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private AppUser GetCurrentUser()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public IActionResult Dashboard()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return RedirectToAction("Logout", "Auth");

            ViewBag.UserRole = currentUser.Role;
            ViewBag.TotalTasks = _context.Tasks.Count();
            ViewBag.CompletedTasks = _context.Tasks.Count(t => t.IsCompleted);
            ViewBag.TotalDevelopers = _context.Users.Count(u => u.Role == "Gelistirici");
            ViewBag.MyTotalTasks = _context.Tasks.Count(t => t.AppUserId == currentUser.Id);
            ViewBag.MyCompletedTasks = _context.Tasks.Count(t => t.AppUserId == currentUser.Id && t.IsCompleted);
            ViewBag.ActiveTeamTasks = _context.Tasks.Count(t => !t.IsCompleted);
            
            return View();
        }

        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return RedirectToAction("Logout", "Auth");

            List<TodoTask> tasks;
            ViewBag.UserRole = currentUser.Role;

            DateTime urgentThreshold = DateTime.Now.AddHours(72);

            // 1. GELİŞTİRİCİ VERİLERİ
            if (currentUser.Role == "Gelistirici")
            {
                tasks = _context.Tasks.Where(t => t.AppUserId == currentUser.Id && !t.IsCompleted).OrderBy(t => t.DueDate).ToList();
                ViewBag.MyTotal = tasks.Count();
                ViewBag.MyCompleted = _context.Tasks.Count(t => t.AppUserId == currentUser.Id && t.IsCompleted);
                ViewBag.MyUrgent = tasks.Count(t => t.DueDate < urgentThreshold);
            }
            // 2. PROJE ŞEFİ VERİLERİ
            else if (currentUser.Role == "Proje Sefi")
            {
                tasks = _context.Tasks.Where(t => !t.IsCompleted).OrderBy(t => t.DueDate).ToList();
                ViewBag.ActiveTeamTasks = tasks.Count();
                ViewBag.UrgentTeamTasks = tasks.Count(t => t.DueDate < urgentThreshold);
                ViewBag.TotalDevelopers = _context.Users.Count(u => u.Role == "Gelistirici");
            }
            // 3. ADMIN VERİLERİ (Sadece istatistikler, liste boş)
            else
            {
                tasks = new List<TodoTask>();
                ViewBag.TotalTasks = _context.Tasks.Count();
                ViewBag.CompletedTasks = _context.Tasks.Count(t => t.IsCompleted);
                ViewBag.UrgentTasks = _context.Tasks.Count(t => t.DueDate < urgentThreshold && !t.IsCompleted);
            }

            return View(tasks);
        }

        public IActionResult Create()
        {
            var currentUser = GetCurrentUser();
            // Görev atamayı sadece Proje Şefi yapabilir!
            if (currentUser.Role != "Proje Sefi") return RedirectToAction("Index");

            ViewBag.Developers = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoTask task, IFormFile? uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                task.AttachedFilePath = "/uploads/" + uniqueFileName;
            }

            task.CreatedAt = DateTime.Now;
            task.Status = "Devam Ediyor";
            _context.Tasks.Add(task);

            // YENİ GÖREVİ DE LOGLARA DÜŞÜRELİM
            var assignedUser = _context.Users.Find(task.AppUserId);
            if (assignedUser != null)
            {
                _context.SystemLogs.Add(new SystemLog { ActionType = "YENİ GÖREV", Message = $"Proje Şefi, {assignedUser.FullName} adlı kişiye '{task.Title}' görevini atadı." });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // YENİ EKLENEN ONAY VE REVİZE ALGORİTMASI (Eski Delete metodunun yerini aldı)
        public IActionResult ChangeStatus(int id, string newStatus)
        {
            var task = _context.Tasks.Include(t => t.AppUser).FirstOrDefault(t => t.Id == id);
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (task != null)
            {
                task.Status = newStatus;

                // Eğer Şef "Tamamlandı" derse ancak o zaman sistemden düşer
                if (newStatus == "Tamamlandı" && currentUserRole == "Proje Sefi")
                {
                    task.IsCompleted = true;
                    _context.SystemLogs.Add(new SystemLog { ActionType = "GÖREV ONAYI", Message = $"Proje Şefi, {task.AppUser?.FullName} adlı kişinin '{task.Title}' görevini ONAYLADI." });
                }
                // Geliştirici onaya gönderirse
                else if (newStatus == "Onay Bekliyor")
                {
                    _context.SystemLogs.Add(new SystemLog { ActionType = "GÖREV TESLİMİ", Message = $"{task.AppUser?.FullName}, '{task.Title}' görevini Şefin onayına sundu." });
                }
                // Şef revize isterse
                else if (newStatus == "Revize")
                {
                    _context.SystemLogs.Add(new SystemLog { ActionType = "GÖREV İADESİ", Message = $"Proje Şefi, '{task.Title}' görevini eksik bularak {task.AppUser?.FullName} adlı kişiye REVİZE için geri gönderdi." });
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var task = _context.Tasks.Include(t => t.AppUser).FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Proje Sefi") return RedirectToAction("Index");

            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound();

            ViewBag.Developers = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TodoTask updatedTask, IFormFile? uploadedFile)
        {
            var existingTask = _context.Tasks.Find(id);
            if (existingTask == null) return NotFound();

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.EstimatedTimeHours = updatedTask.EstimatedTimeHours;
            existingTask.AppUserId = updatedTask.AppUserId;

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                existingTask.AttachedFilePath = "/uploads/" + uniqueFileName;
            }

            _context.SystemLogs.Add(new SystemLog { ActionType = "GÖREV GÜNC.", Message = $"Proje Şefi, '{existingTask.Title}' görevini güncelledi." });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Proje Sefi") return RedirectToAction("Index");

            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SystemLogs.Add(new SystemLog { ActionType = "GÖREV İPTAL", Message = $"Proje Şefi, '{task.Title}' görevini sistemden sildi." });
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}