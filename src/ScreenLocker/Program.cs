namespace ScreenLocker;

using ScreenLocker.Config;
using ScreenLocker.Forms;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var config = LockConfig.Load();
        var messageConfig = MessageConfig.Load();

        var lockForm = new LockScreenForm(config, messageConfig);
        Application.Run(lockForm);
    }
}
