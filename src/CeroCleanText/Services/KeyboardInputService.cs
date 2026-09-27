using System.Runtime.InteropServices;

namespace CeroCleanText.Services;

internal static class KeyboardInputService
{
    private const ushort VkControl = 0x11;
    private const ushort VkC = 0x43;
    private const ushort VkV = 0x56;
    private const uint KeyUp = 0x0002;
    private const uint InputKeyboard = 1;

    public static bool Copy() => SendChord(VkControl, VkC);

    public static bool Paste() => SendChord(VkControl, VkV);

    private static bool SendChord(ushort modifier, ushort key)
    {
        var inputs = new[]
        {
            Key(modifier, false),
            Key(key, false),
            Key(key, true),
            Key(modifier, true)
        };

        return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>()) == inputs.Length;
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
        [FieldOffset(0)]
        public KEYBDINPUT ki;
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

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint cInputs, INPUT[] pInputs, int cbSize);
}
