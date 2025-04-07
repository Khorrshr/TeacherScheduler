

// App.xaml.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;
using TempusNexum.Data;

namespace TempusNexum
{
    public partial class App : System.Windows.Application
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
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
            services.AddSingleton<LoginWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("App.xaml.cs: OnStartup called");
            var loginWindow = _serviceProvider.GetService<LoginWindow>();
            if (loginWindow == null)
            {
                Console.WriteLine("App.xaml.cs: Failed to resolve LoginWindow");
                return;
            }
            Console.WriteLine("App.xaml.cs: Showing LoginWindow");
            loginWindow.Show();
            Console.WriteLine("App.xaml.cs: LoginWindow shown");
        }
    }
}


/*
// App.xaml.cs

using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;
using TempusNexum.Data;
using TempusNexum.Models;
using BCrypt.Net;

namespace TempusNexum
{
    public partial class App : System.Windows.Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            try
            {
                Console.WriteLine("App.xaml.cs: Constructor called");
                var services = new ServiceCollection();
                ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();
                Console.WriteLine("App.xaml.cs: Constructor completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"App.xaml.cs: Error in constructor: {ex.Message}");
                System.Windows.MessageBox.Show($"Error in constructor: {ex.Message}");
                throw;
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            try
            {
                Console.WriteLine("App.xaml.cs: Configuring services");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
                services.AddSingleton<LoginWindow>();
                Console.WriteLine("App.xaml.cs: Services configured");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"App.xaml.cs: Error configuring services: {ex.Message}");
                System.Windows.MessageBox.Show($"Error configuring services: {ex.Message}");
                throw;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
                {
                    Console.WriteLine($"Unhandled exception: {ex.ExceptionObject}");
                    System.Windows.MessageBox.Show($"Unhandled exception: {ex.ExceptionObject}");
                };

                base.OnStartup(e);
                Console.WriteLine("App.xaml.cs: OnStartup called");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<AppDbContext>();
                    if (context == null)
                    {
                        Console.WriteLine("App.xaml.cs: Failed to resolve AppDbContext");
                        return;
                    }
                    if (!context.Users.Any())
                    {
                        Console.WriteLine("App.xaml.cs: Seeding test user");
                        context.Users.Add(new User
                        {
                            Username = "testuser",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                            Roles = "Teacher,Manager,Admin"
                        });
                        context.SaveChanges();
                        Console.WriteLine("App.xaml.cs: Test user seeded");
                    }
                }

                var loginWindow = _serviceProvider.GetService<LoginWindow>();
                if (loginWindow == null)
                {
                    Console.WriteLine("App.xaml.cs: Failed to resolve LoginWindow");
                    return;
                }
                Console.WriteLine("App.xaml.cs: Showing LoginWindow");
                loginWindow.Show();
                Console.WriteLine("App.xaml.cs: LoginWindow shown");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"App.xaml.cs: Error in OnStartup: {ex.Message}");
                System.Windows.MessageBox.Show($"Error in OnStartup: {ex.Message}");
                throw;
            }
        }
    }
}

*/