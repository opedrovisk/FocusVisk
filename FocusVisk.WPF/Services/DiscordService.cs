using DiscordRPC;
using DiscordRPC.Logging;

namespace FocusVisk.Services;

public class DiscordService : IDisposable
{
    private DiscordRpcClient? _client;
    private DateTime _startTime;

    public void Initialize()
    {
        _client = new DiscordRpcClient("1527102674846089256");
        _client.Logger = new ConsoleLogger { Level = LogLevel.Warning };
        _client.Initialize();
        _startTime = DateTime.UtcNow;
    }

    public void UpdatePresence(string details, string state)
    {
        if (_client == null || _client.IsDisposed) return;
        _client.SetPresence(new RichPresence
        {
            Details = details,
            State = state,
            Assets = new Assets
            {
                LargeImageKey = "focusvisk_logo",
                LargeImageText = "FocusVisk",
                SmallImageKey = "focusvisk_icon",
                SmallImageText = "Produtividade"
            },
            Timestamps = new Timestamps(_startTime)
        });
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}