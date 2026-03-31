using GoldBranchAI.Data;
using GoldBranchAI.Models;
using GoldBranchAI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace GoldBranchAI.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _geminiService;

        public AiController(AppDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        private AppUser? GetCurrentUser()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// AI Görev Bölme sayfası (GET)
        /// </summary>
        [HttpGet]
        public IActionResult Breakdown()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return RedirectToAction("Login", "Auth");
            if (currentUser.Role != "Proje Sefi" && currentUser.Role != "Admin")
                return RedirectToAction("Index", "Task");

            ViewBag.Developers = _context.Users.Where(u => u.Role == "Gelistirici").ToList();
            ViewBag.UserRole = currentUser.Role;
            return View();
        }

        /// <summary>
        /// Gemini API'ye proje açıklaması gönder, alt görevleri al (POST - AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return Unauthorized();
            if (currentUser.Role != "Proje Sefi" && currentUser.Role != "Admin")
                return Forbid();

            if (string.IsNullOrWhiteSpace(request.ProjectDescription))
                return BadRequest(new { error = "Proje açıklaması boş olamaz." });

            try
            {
                var (tasks, rawJson) = await _geminiService.BreakdownProjectAsync(request.ProjectDescription);

                // Breakdown kaydını veritabanına kaydet
                var breakdown = new AiTaskBreakdown
                {
                    ProjectDescription = request.ProjectDescription,
                    GeneratedJson = rawJson,
                    SubTaskCount = tasks.Count,
                    CreatedByUserId = currentUser.Id,
                    CreatedAt = DateTime.Now
                };
                _context.AiTaskBreakdowns.Add(breakdown);

                _context.SystemLogs.Add(new SystemLog
                {
                    ActionType = "AI ANALİZ",
                    Message = $"{currentUser.FullName}, AI ile bir projeyi {tasks.Count} alt göreve böldü."
                });

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    breakdownId = breakdown.Id,
                    tasks = tasks.Select(t => new
                    {
                        title = t.Title,
                        description = t.Description,
                        estimatedHours = t.EstimatedHours,
                        priority = t.Priority,
                        deadlineDays = t.DeadlineDays
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Onaylanan alt görevleri TodoTask tablosuna toplu kaydet (POST - AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ApplyBreakdown([FromBody] ApplyRequest request)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return Unauthorized();
            if (currentUser.Role != "Proje Sefi" && currentUser.Role != "Admin")
                return Forbid();

            var breakdown = _context.AiTaskBreakdowns.Find(request.BreakdownId);
            if (breakdown == null) return NotFound();
            if (breakdown.IsApplied) return BadRequest(new { error = "Bu analiz zaten sisteme aktarıldı." });

            var assignedUser = _context.Users.Find(request.AssignToUserId);
            if (assignedUser == null) return BadRequest(new { error = "Geçersiz geliştirici seçimi." });

            int addedCount = 0;
            foreach (var task in request.Tasks)
            {
                var newTask = new TodoTask
                {
                    Title = task.Title,
                    Description = task.Description,
                    EstimatedTimeHours = task.EstimatedHours,
                    AppUserId = request.AssignToUserId,
                    DueDate = DateTime.Now.AddDays(task.DeadlineDays > 0 ? task.DeadlineDays : 7),
                    Status = "Devam Ediyor",
                    CreatedAt = DateTime.Now
                };
                _context.Tasks.Add(newTask);
                addedCount++;
            }

            breakdown.IsApplied = true;

            _context.SystemLogs.Add(new SystemLog
            {
                ActionType = "AI GÖREV AKTARIMI",
                Message = $"{currentUser.FullName}, AI analizinden {addedCount} görevi {assignedUser.FullName} adlı kişiye toplu atadı."
            });

            await _context.SaveChangesAsync();

            return Json(new { success = true, addedCount });
        }

        /// <summary>
        /// Geliştiriciler için AI Araştırma/Sohbet sayfası (GET)
        /// </summary>
        [HttpGet]
        public IActionResult Research()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return RedirectToAction("Login", "Auth");
            return View();
        }

        /// <summary>
        /// Geliştirici soruları için AI'a istek at ve veritabanına kaydet
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AskResearch([FromBody] AskRequest request)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return Unauthorized();

            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest(new { error = "Soru boş olamaz." });

            var answer = await _geminiService.AskDeveloperQuestionAsync(request.Question);

            // Veritabanına logla
            var log = new AiResearchLog
            {
                AppUserId = currentUser.Id,
                RequestPrompt = request.Question,
                ResponseMarkdown = answer,
                CreatedAt = DateTime.Now
            };
            
            _context.AiResearchLogs.Add(log);
            await _context.SaveChangesAsync();

            return Json(new { success = true, answer = answer });
        }
    }

    // --- Request DTO'ları ---
    public class AnalyzeRequest
    {
        public string ProjectDescription { get; set; } = string.Empty;
    }

    public class AskRequest
    {
        public string Question { get; set; } = string.Empty;
    }

    public class ApplyRequest
    {
        public int BreakdownId { get; set; }
        public int AssignToUserId { get; set; }
        public List<ApplyTaskItem> Tasks { get; set; } = new();
    }

    public class ApplyTaskItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EstimatedHours { get; set; }
        public int DeadlineDays { get; set; }
    }
}
