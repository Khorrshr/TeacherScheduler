// LoginWindow.xaml.cs
using Microsoft.EntityFrameworkCore;
using TeacherScheduler.Data;
using System.Windows;
using BCrypt.Net;

namespace TeacherScheduler
{
    public partial class LoginWindow : Window
    {
        private readonly AppDbContext _context;

        public LoginWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var user = _context.Users
                .FirstOrDefault(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var mainWindow = new MainWindow(user);
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}