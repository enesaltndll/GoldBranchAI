using System.ComponentModel.DataAnnotations;

namespace GoldBranchAI.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        // YENİ: Sistem içi giriş için şifre alanı
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "Gelistirici";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TodoTask>? TodoTasks { get; set; }
    }
}