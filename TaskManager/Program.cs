using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Services;
using TaskManagement.Repositories;
using TaskManagement.Forms;
using NLog;
using NLog.Extensions.Hosting;

namespace TaskManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
                context.Database.EnsureCreated();
            }

            var form1 = host.Services.GetRequiredService<Form1>();
            Application.Run(form1);
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .UseNLog() // Добавляем NLog
                .ConfigureServices((context, services) =>
                {
                    // Database
                    services.AddDbContext<TaskDbContext>(options =>
                        options.UseSqlite("Data Source=tasks.db"));

                    // Repositories
                    services.AddScoped<ITaskRepository, TaskRepository>();

                    // Services
                    services.AddScoped<ITaskService, TaskService>();

                    // Forms
                    services.AddTransient<Form1>();
                    services.AddTransient<TaskEditForm>();
                });
    }
}
