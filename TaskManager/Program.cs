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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
                context.Database.EnsureCreated();
            }

            var mainForm = host.Services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
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
                    services.AddTransient<MainForm>();
                    services.AddTransient<TaskEditForm>();
                });
    }
}
