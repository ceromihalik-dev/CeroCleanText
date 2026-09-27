using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CeroCleanText.Services;

internal static class KeyboardInputService
{
    private const ushort VkControl = 0x11;
    private const ushort VkC = 0x43;
    private const ushort VkV = 0x56;
    private const uint KeyUp = 0x0002;
    private const uint InputKeyboard = 1;

    public static string? LastError { get; private set; }

    public static async Task<bool> WaitForHotkeyModifiersReleasedAsync(int timeoutMs = 1000)
    {
        var started = Environment.TickCount64;
        while (IsKeyDown(VkControl) || IsKeyDown(0x12)) // VK_MENU / Alt
        {
            if (Environment.TickCount64 - started >= timeoutMs)
            {
                LastError = "Ctrl/Alt were not released before copy timeout.";
                return false;
            }

            await Task.Delay(10);
        }

        return true;
    }

    public static bool Copy() => SendChord(VkControl, VkC);

    public static bool Paste() => SendChord(VkControl, VkV);

    private static bool SendChord(ushort modifier, ushort key)
    {
        LastError = null;

        var inputs = new[]
        {
            Key(modifier, false),
            Key(key, false),
            Key(key, true),
            Key(modifier, true)
        };

        var sent = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        if (sent == inputs.Length)
            return true;

        var error = Marshal.GetLastWin32Error();
        LastError = $"SendInput sent {sent}/{inputs.Length}; Win32={error} ({new Win32Exception(error).Message}); INPUT={Marshal.SizeOf<INPUT>()}";
        return false;
    }

    private static INPUT Key(ushort virtualKey, bool keyUp) => new()
    {
        type = InputKeyboard,
        U = new InputUnion
        {
            ki = new KEYBDINPUT
            {
                wVk = virtualKey,
                dwFlags = keyUp ? KeyUp : 0
            }
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    private static bool IsKeyDown(int virtualKey)
        => (GetAsyncKeyState(virtualKey) & 0x8000) != 0;

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint cInputs, INPUT[] pInputs, int cbSize);
}
