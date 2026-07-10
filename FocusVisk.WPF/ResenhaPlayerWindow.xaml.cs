using System.Windows;
using System.Windows.Input;

namespace FocusVisk;

public partial class ResenhaPlayerWindow : Window
{
    private readonly bool _isVideo;

    public ResenhaPlayerWindow(string filePath, bool isVideo)
    {
        InitializeComponent();
        _isVideo = isVideo;
        Player.Source = new Uri(filePath, UriKind.Absolute);

        if (!isVideo)
        {
            Player.LoadedBehavior = System.Windows.Controls.MediaState.Pause;
            Player.Loaded += (_, _) => Player.Play();
        }
    }

    private void Player_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (_isVideo)
        {
            Player.Position = TimeSpan.Zero;
            Player.Play();
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape) Close();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();
}
