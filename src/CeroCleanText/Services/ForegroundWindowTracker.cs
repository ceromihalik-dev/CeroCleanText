using System.Runtime.InteropServices;
using System.Text;

namespace CeroCleanText.Services;

internal sealed class ForegroundWindowTracker : IDisposable
{
    private const uint EventSystemForeground = 0x0003;
    private const uint WineventOutofcontext = 0x0000;
    private readonly WinEventDelegate _callback;
    private readonly IntPtr _hook;

    public IntPtr LastExternalWindow { get; private set; }

    public ForegroundWindowTracker()
    {
        LastExternalWindow = GetForegroundWindow();
        _callback = OnForegroundChanged;
        _hook = SetWinEventHook(
            EventSystemForeground,
            EventSystemForeground,
            IntPtr.Zero,
            _callback,
            0,
            0,
            WineventOutofcontext);
    }

    private void OnForegroundChanged(
        IntPtr hWinEventHook,
        uint eventType,
        IntPtr hwnd,
        int idObject,
        int idChild,
        uint idEventThread,
        uint eventTime)
    {
        if (hwnd == IntPtr.Zero)
            return;

        GetWindowThreadProcessId(hwnd, out var processId);
        if (processId == Environment.ProcessId)
            return;

        LastExternalWindow = hwnd;
    }

    public void Dispose()
    {
        if (_hook != IntPtr.Zero)
            UnhookWinEvent(_hook);
    }

    private delegate void WinEventDelegate(
        IntPtr hWinEventHook,
        uint eventType,
        IntPtr hwnd,
        int idObject,
        int idChild,
        uint idEventThread,
        uint eventTime);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWinEventHook(
        uint eventMin,
        uint eventMax,
        IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc,
        uint idProcess,
        uint idThread,
        uint dwFlags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
}
