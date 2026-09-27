using CeroCleanText.Services;

namespace CeroCleanText;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var singleInstance = new Mutex(true, @"Local\CeroCleanText", out var createdNew);
        if (!createdNew)
            return;

        StartupService.EnsureEnabled();
        Application.Run(new TrayApplicationContext());
    }
}
