using FocusVisk.Data;
using FocusVisk.Native;
using FocusVisk.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

namespace FocusVisk;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ToastRegistration.Register();

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var taskSvc = Services.GetRequiredService<TaskService>();
        _ = taskSvc.ResetRecurringTasksAsync();

        var discordSvc = Services.GetRequiredService<DiscordService>();
        discordSvc.Initialize();

        var mainWindow = Services.GetRequiredService<MainWindow>();

        var startMinimized = e.Args.Contains("--background", StringComparer.OrdinalIgnoreCase);

        mainWindow.Show();
        if (startMinimized)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }
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
        services.AddSingleton<StartupService>();
        services.AddSingleton<DiscordService>();
        services.AddSingleton<SearchService>();
        services.AddSingleton<DataExportService>();
        services.AddSingleton<ShortcutService>();
        services.AddSingleton<TagService>();

        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        services.AddSingleton<MainWindow>();
    }
}