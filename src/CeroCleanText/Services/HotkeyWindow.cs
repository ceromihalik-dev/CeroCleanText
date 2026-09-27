using System.Runtime.InteropServices;

namespace CeroCleanText.Services;

internal sealed class HotkeyWindow : NativeWindow, IDisposable
{
    private const int WmHotkey = 0x0312;
    private const int HotkeyId = 0x4343;

    public event EventHandler? Pressed;

    public uint Modifiers { get; private set; }
    public uint Key { get; private set; }

    public HotkeyWindow()
    {
        CreateHandle(new CreateParams());
        if (!TryRegister(UserSettingsService.HotkeyModifiers, UserSettingsService.HotkeyKey))
        {
            if (!TryRegister(0x0003, 0x54))
                throw new InvalidOperationException("The CeroCleanText global hotkey could not be registered.");
        }
    }

    public bool TryRegister(uint modifiers, uint key)
    {
        if (Handle == IntPtr.Zero)
            return false;

        if (Modifiers != 0 || Key != 0)
            UnregisterHotKey(Handle, HotkeyId);

        if (RegisterHotKey(Handle, HotkeyId, modifiers, key))
        {
            Modifiers = modifiers;
            Key = key;
            return true;
        }

        if (Modifiers != 0 || Key != 0)
            RegisterHotKey(Handle, HotkeyId, Modifiers, Key);

        return false;
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WmHotkey && m.WParam.ToInt32() == HotkeyId)
            Pressed?.Invoke(this, EventArgs.Empty);

        base.WndProc(ref m);
    }

    public void Dispose()
    {
        if (Handle != IntPtr.Zero)
        {
            UnregisterHotKey(Handle, HotkeyId);
            DestroyHandle();
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}
