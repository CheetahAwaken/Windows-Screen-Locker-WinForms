namespace ScreenLocker.Protection;

using Microsoft.Win32;

public static class TaskManagerDisabler
{
    private const string PolicyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
    private const string ValueName = "DisableTaskMgr";

    public static void Disable()
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(PolicyPath, true);
            key?.SetValue(ValueName, 1, RegistryValueKind.DWord);
        }
        catch { }
    }

    public static void Enable()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(PolicyPath, true);
            key?.DeleteValue(ValueName, false);
        }
        catch { }
    }

    public static bool IsDisabled()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(PolicyPath);
            var value = key?.GetValue(ValueName);
            return value is int intValue && intValue == 1;
        }
        catch
        {
            return false;
        }
    }
}
