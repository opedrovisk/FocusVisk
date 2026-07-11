using FocusVisk.Data;
using FocusVisk.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace FocusVisk;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FocusViskDb;Trusted_Connection=True;"),
            ServiceLifetime.Transient);

        services.AddSingleton<PomodoroService>();
        services.AddSingleton<TaskService>();
        services.AddSingleton<CalendarService>();
        services.AddSingleton<NotesService>();
        services.AddSingleton<ThemeService>();
        services.AddSingleton<FocusBlockerService>();
        services.AddSingleton<StatsService>();
        services.AddSingleton<FinancasService>();
        services.AddSingleton<AlertService>();
        services.AddSingleton<HabitService>();

        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        services.AddSingleton<MainWindow>();
    }
}