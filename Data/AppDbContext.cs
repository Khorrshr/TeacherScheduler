// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using TempusNexum.Models;

namespace TempusNexum.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<TeacherGroup> TeacherGroups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Timetable> Timetables { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherGroup>()
                .HasKey(tg => new { tg.TeacherID, tg.GroupID });

            modelBuilder.Entity<TeacherGroup>()
                .HasOne(tg => tg.Teacher)
                .WithMany(t => t.TeacherGroups)
                .HasForeignKey(tg => tg.TeacherID);

            modelBuilder.Entity<TeacherGroup>()
                .HasOne(tg => tg.Group)
                .WithMany(g => g.TeacherGroups)
                .HasForeignKey(tg => tg.GroupID);
        }
    }
}