using Microsoft.Win32;

namespace CeroCleanText.Services;

internal static class UserSettingsService
{
    private const string KeyPath = @"Software\CeroCleanText";
    private const string NotifyValue = "ShowCleanNotification";

    public static bool ShowCleanNotification
    {
        get
        {
            using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
            return key?.GetValue(NotifyValue) is not int value || value != 0;
        }
        set
        {
            using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
            key.SetValue(NotifyValue, value ? 1 : 0, RegistryValueKind.DWord);
        }
    }
}
