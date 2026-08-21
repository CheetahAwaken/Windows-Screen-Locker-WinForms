namespace ScreenLocker.Protection;

using System.Runtime.InteropServices;

public static class AltTabBlocker
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CONTROL = 0x0002;
    private const uint VK_TAB = 0x09;
    private const uint VK_ESCAPE = 0x1B;
    private const uint VK_DELETE = 0x2E;

    private static readonly List<int> RegisteredHotKeys = [];

    public static void Block()
    {
        RegisterBlockingHotKey(1, MOD_ALT, VK_TAB);
        RegisterBlockingHotKey(2, MOD_ALT, VK_ESCAPE);
        RegisterBlockingHotKey(3, MOD_CONTROL | MOD_ALT, VK_DELETE);
    }

    public static void Unblock()
    {
        foreach (int id in RegisteredHotKeys)
        {
            UnregisterHotKey(IntPtr.Zero, id);
        }
        RegisteredHotKeys.Clear();
    }

    private static void RegisterBlockingHotKey(int id, uint modifiers, uint key)
    {
        if (RegisterHotKey(IntPtr.Zero, id, modifiers, key))
        {
            RegisteredHotKeys.Add(id);
        }
    }
}
