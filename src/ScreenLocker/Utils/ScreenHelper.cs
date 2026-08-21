namespace ScreenLocker.Utils;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class ScreenHelper
{
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static Size GetPrimaryScreenSize()
    {
        return Screen.PrimaryScreen?.Bounds.Size ?? new Size(1920, 1080);
    }

    public static Rectangle GetTotalScreenBounds()
    {
        int left = int.MaxValue, top = int.MaxValue;
        int right = int.MinValue, bottom = int.MinValue;

        foreach (var screen in Screen.AllScreens)
        {
            left = Math.Min(left, screen.Bounds.Left);
            top = Math.Min(top, screen.Bounds.Top);
            right = Math.Max(right, screen.Bounds.Right);
            bottom = Math.Max(bottom, screen.Bounds.Bottom);
        }

        return new Rectangle(left, top, right - left, bottom - top);
    }

    public static void BringToFront(IntPtr handle)
    {
        ShowWindow(handle, 9); // SW_RESTORE
        SetForegroundWindow(handle);
    }

    public static void CoverAllScreens(Form mainForm)
    {
        mainForm.Bounds = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1920, 1080);

        foreach (var screen in Screen.AllScreens)
        {
            if (screen.Primary)
                continue;

            var blocker = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                TopMost = true,
                ShowInTaskbar = false,
                Bounds = screen.Bounds
            };
            blocker.Show();
        }
    }

    public static int GetScreenCount()
    {
        return Screen.AllScreens.Length;
    }
}
