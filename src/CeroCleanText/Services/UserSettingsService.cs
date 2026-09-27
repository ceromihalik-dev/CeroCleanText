using Microsoft.Win32;

namespace CeroCleanText.Services;

internal static class UserSettingsService
{
    private const string KeyPath = @"Software\CeroCleanText";
    private const string NotifyValue = "ShowCleanNotification";
    private const string HotkeyModifiersValue = "HotkeyModifiers";
    private const string HotkeyKeyValue = "HotkeyKey";

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
    public static uint HotkeyModifiers
    {
        get
        {
            using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
            return Convert.ToUInt32(key?.GetValue(HotkeyModifiersValue) ?? 0x0003); // Alt + Ctrl
        }
        set
        {
            using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
            key.SetValue(HotkeyModifiersValue, value, RegistryValueKind.DWord);
        }
    }

    public static uint HotkeyKey
    {
        get
        {
            using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
            return Convert.ToUInt32(key?.GetValue(HotkeyKeyValue) ?? 0x54); // T
        }
        set
        {
            using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
            key.SetValue(HotkeyKeyValue, value, RegistryValueKind.DWord);
        }
    }
}

