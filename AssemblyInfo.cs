// Models/User.cs
namespace TeacherScheduler.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Roles { get; set; } // Comma-separated, e.g., "Teacher,Manager"
    }
}

// Models/Teacher.cs
namespace TeacherScheduler.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public int UserID { get; set; }
        public int ObligatoryHours { get; set; }
        public string Name { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}

// Models/Group.cs
namespace TeacherScheduler.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}

// Models/TeacherGroup.cs
namespace TeacherScheduler.Models
{
    public class TeacherGroup
    {
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public int GroupID { get; set; }
        public Group Group { get; set; }
    }
}

// Models/Schedule.cs
namespace TeacherScheduler.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int TeacherID { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Status { get; set; } // "InPerson", "Online", "Unavailable"
        public Teacher Teacher { get; set; }
    }
}

// Models/Course.cs
namespace TeacherScheduler.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Name { get; set; }
        public int RequiredHours { get; set; }
        public int GroupID { get; set; }
        public Group Group { get; set; }
        public int StudyProgramID { get; set; }
        public StudyProgram StudyProgram { get; set; }
    }
}

// Models/StudyProgram.cs
namespace TeacherScheduler.Models
{
    public class StudyProgram
    {
        public int StudyProgramID { get; set; }
        public string Name { get; set; }
        public string SubjectStatus { get; set; } // "Pending", "Complete", "InProcess"
        public List<Course> Courses { get; set; }
    }
}

// Models/Facility.cs
namespace TeacherScheduler.Models
{
    public class Facility
    {
        public int FacilityID { get; set; }
        public string Name { get; set; }
        public DateTime TimeSlot { get; set; }
        public bool IsAvailable { get; set; }
    }
}

// Models/Timetable.cs
namespace TeacherScheduler.Models
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public DateTime TimeSlot { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int FacilityID { get; set; }
        public Facility Facility { get; set; }
        public int CreatedBy { get; set; }
        public User Creator { get; set; }
    }
}


// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using TeacherScheduler.Models;

namespace TeacherScheduler.Data
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

{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TeacherSchedulerDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
    }
}

// App.xaml.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TeacherScheduler.Data;

namespace TeacherScheduler
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    ConfigurationManager.Configuration.GetConnectionString("DefaultConnection")));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}