using FocusVisk.Services;
using System.Windows;

namespace FocusVisk;

public partial class MainWindow : Window
{
    private readonly PomodoroService _pomodoro;
    private readonly AlertService _alertService;
    private bool _isExiting = false;

    public MainWindow(PomodoroService pomodoro, AlertService alertService)
    {
        InitializeComponent();
        _pomodoro = pomodoro;
        _alertService = alertService;

        Resources.Add("services", App.Services);

        _pomodoro.OnTick += (remaining, isRunning) =>
        {
            Dispatcher.Invoke(() =>
            {
                TrayIcon.ToolTipText = isRunning
                    ? $"FocusVisk — {remaining:mm\\:ss} restantes"
                    : "FocusVisk";
            });
        };
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
            Hide();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!_isExiting)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
        }
    }

    private void TrayIcon_DoubleClick(object sender, RoutedEventArgs e) => ShowApp();
    private void TrayOpen_Click(object sender, RoutedEventArgs e) => ShowApp();

    private void TrayPomodoro_Click(object sender, RoutedEventArgs e)
    {
        _pomodoro.Start();
        ShowApp();
    }

    private async void TrayNewTask_Click(object sender, RoutedEventArgs e)
    {
        ShowApp();
        await Task.Delay(300);
        await (BlazorView.WebView?.CoreWebView2?.ExecuteScriptAsync(
            "window.focusApp?.navigateTo('tasks')") ?? Task.FromResult(""));
    }

    private void TrayExit_Click(object sender, RoutedEventArgs e)
    {
        _isExiting = true;
        TrayIcon.Dispose();
        Application.Current.Shutdown();
    }

    private void ShowApp()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }
}