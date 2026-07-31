using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace FocusVisk.Native;

public static class ToastRegistration
{
    public const string AppId = "FocusVisk.DesktopApp";

    [DllImport("shell32.dll", SetLastError = true)]
    private static extern int SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string appId);

    public static void Register()
    {
        SetCurrentProcessExplicitAppUserModelID(AppId);
        RegisterAppInRegistry();
    }

    private static void RegisterAppInRegistry()
    {
        try
        {
            var keyPath = $@"SOFTWARE\Classes\AppUserModelId\{AppId}";
            using var key = Registry.CurrentUser.CreateSubKey(keyPath);

            key.SetValue("DisplayName", "FocusVisk", RegistryValueKind.String);

            var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icon.ico");
            if (File.Exists(iconPath))
                key.SetValue("IconUri", iconPath, RegistryValueKind.String);

            key.SetValue("IconBackgroundColor", "FF7C6AF7", RegistryValueKind.String);
        }
        catch { }
    }
}