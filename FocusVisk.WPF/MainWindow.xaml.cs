using FocusVisk.Services;
using System.Windows;

namespace FocusVisk;

public partial class MainWindow : Window
{
    private readonly PomodoroService _pomodoro;
    private bool _isExiting = false;

    public MainWindow(PomodoroService pomodoro)
    {
        InitializeComponent();
        _pomodoro = pomodoro;

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
        {
            Hide();
        }
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

    private void TrayNewTask_Click(object sender, RoutedEventArgs e)
    {
        ShowApp();
        BlazorView.WebView?.CoreWebView2?.ExecuteScriptAsync(
            "window.focusApp?.navigateTo('tasks')");
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