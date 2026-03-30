using GoldBranchAI.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldBranchAI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<TodoTask> Tasks { get; set; }
        public DbSet<DailyTimeLog> DailyTimeLogs { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; } // YENİ EKLENEN SATIR

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>().HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChatMessage>().HasOne(m => m.Receiver).WithMany().HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}