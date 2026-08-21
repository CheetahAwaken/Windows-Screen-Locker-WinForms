namespace ScreenLocker.Protection;

using System.Diagnostics;
using Microsoft.Win32;

public static class SafeBootPreventer
{
    public static void PreventSafeMode()
    {
        DisableF8Boot();
        SetBootPolicy();
    }

    public static void RestoreSafeMode()
    {
        RestoreF8Boot();
    }

    private static void DisableF8Boot()
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "bcdedit",
                Arguments = "/set {default} bootmenupolicy standard",
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process?.WaitForExit(5000);
        }
        catch { }
    }

    private static void RestoreF8Boot()
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "bcdedit",
                Arguments = "/set {default} bootmenupolicy legacy",
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process?.WaitForExit(5000);
        }
        catch { }
    }

    private static void SetBootPolicy()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\Control\SafeBoot", true);
            key?.SetValue("AlternateShell", "cmd.exe");
        }
        catch { }
    }
}
