// MainWindow.xaml.cs
using Microsoft.EntityFrameworkCore;
using TempusNexum.Data;
using TempusNexum.Models;
using TempusNexum.Services;
using System.Linq;
using System.Windows;
using System.IO;

namespace TempusNexum
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;
        private readonly AppDbContext _context;

        public MainWindow(User currentUser, AppDbContext context)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = context;
            ConfigureAccess();
            LoadSchedules();
        }

        private void ConfigureAccess()
        {
            var roles = _currentUser.Roles.Split(',').ToList();
            if (!roles.Contains("Manager"))
            {
                // Hide Manager tab (index 1)
                TabControl.Items.RemoveAt(1);
            }
            if (!roles.Contains("Admin"))
            {
                // Hide Admin tab (index 2, or index 1 if Manager tab was removed)
                int adminTabIndex = roles.Contains("Manager") ? 2 : 1;
                if (TabControl.Items.Count > adminTabIndex)
                {
                    TabControl.Items.RemoveAt(adminTabIndex);
                }
            }
        }

        private void LoadSchedules()
        {
            var teacher = _context.Teachers
                .FirstOrDefault(t => t.UserID == _currentUser.UserID);
            if (teacher != null)
            {
                var schedules = _context.Schedules
                    .Where(s => s.TeacherID == teacher.TeacherID)
                    .ToList();
                ScheduleGrid.ItemsSource = schedules;
            }
        }

        private void AddScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            var teacher = _context.Teachers
                .FirstOrDefault(t => t.UserID == _currentUser.UserID);
            if (teacher != null)
            {
                var schedule = new Schedule
                {
                    TeacherID = teacher.TeacherID,
                    TimeSlot = DateTime.Now, // Replace with user input
                    Status = StatusComboBox.SelectedItem?.ToString() ?? "Unavailable"
                };
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
                LoadSchedules();
            }
        }

        private void GenerateTimetableButton_Click(object sender, RoutedEventArgs e)
        {
            var scheduler = new Scheduler(_context, _currentUser);
            var schedules = _context.Schedules.ToList();
            var courses = _context.Courses.ToList();
            var facilities = _context.Facilities.ToList();

            var timetable = scheduler.GenerateTimetable(schedules, courses, facilities);
            var conflicts = scheduler.GetConflicts(timetable);

            if (conflicts.Any())
            {
                System.Windows.MessageBox.Show("Conflicts found:\n" + string.Join("\n", conflicts));
            }
            else
            {
                _context.Timetables.AddRange(timetable);
                _context.SaveChanges();
                System.Windows.MessageBox.Show("Timetable generated successfully!");
            }
        }

        private void PrintTimetableButton_Click(object sender, RoutedEventArgs e)
        {
            var timetables = _context.Timetables
                .Include(t => t.Teacher)
                .Include(t => t.Course)
                .Include(t => t.Facility)
                .ToList();

            using (var writer = new StreamWriter("Timetable.txt"))
            {
                foreach (var entry in timetables)
                {
                    writer.WriteLine($"{entry.TimeSlot}: {entry.Teacher.Name} teaches {entry.Course.Name} in {entry.Facility.Name}");
                }
            }

            System.Windows.MessageBox.Show("Timetable exported to Timetable.txt");
        }

        private void ManageUsersButton_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for managing users
            System.Windows.MessageBox.Show("User management functionality coming soon...");
        }
    }
}