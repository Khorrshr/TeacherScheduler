// LoginWindow.xaml.cs
using Microsoft.EntityFrameworkCore;
using TempusNexum.Data;
using TempusNexum.Models;
using System.Windows;
using BCrypt.Net;

namespace TempusNexum
{
    public partial class LoginWindow : Window
    {
        private readonly AppDbContext _context;

        public LoginWindow(AppDbContext context)
        {
            try
            {
                Console.WriteLine("LoginWindow.xaml.cs: Constructor called");
                _context = context;
                InitializeComponent();
                Console.WriteLine("LoginWindow.xaml.cs: InitializeComponent completed");

                this.WindowState = WindowState.Normal;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.Visibility = Visibility.Visible;
                Console.WriteLine("LoginWindow.xaml.cs: Window visibility set");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginWindow.xaml.cs: Error in constructor: {ex.Message}");
                System.Windows.MessageBox.Show($"Error initializing LoginWindow: {ex.Message}");
                throw;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    System.Windows.MessageBox.Show("Database context not available. Login disabled for debugging.");
                    return;
                }

                var username = UsernameTextBox.Text;
                var password = PasswordBox.Password;

                var user = _context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    var mainWindow = new MainWindow(user, _context);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginWindow.xaml.cs: Error in LoginButton_Click: {ex.Message}");
                System.Windows.MessageBox.Show($"Error during login: {ex.Message}");
            }
        }
    }
}