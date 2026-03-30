using System.ComponentModel.DataAnnotations;

namespace GoldBranchAI.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }

        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // YENİ EKLENEN ONAY STATÜSÜ: (Devam Ediyor, Onay Bekliyor, Revize, Tamamlandı)
        public string Status { get; set; } = "Devam Ediyor";

        public int EstimatedTimeHours { get; set; }
        public int SpentTimeMinutes { get; set; } = 0;
        public string? AttachedFilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}