using CeroCleanText.Core;
using System.Runtime.InteropServices;

namespace CeroCleanText.Services;

internal static class SelectionCleaner
{
    private const int ClipboardRetries = 8;
    private const int RetryDelayMs = 25;
    private const int CopyWaitMs = 120;
    private const int PasteWaitMs = 180;

    public static async Task<bool> CleanSelectionAsync(IntPtr targetWindow)
    {
        var backup = await TryGetClipboardDataAsync();

        try
        {
            if (targetWindow != IntPtr.Zero)
            {
                SetForegroundWindow(targetWindow);
                await Task.Delay(80);
            }

            await TryClearClipboardAsync();
            SendKeys.SendWait("^c");
            await Task.Delay(CopyWaitMs);

            var selectedText = await TryGetTextAsync();
            if (selectedText is null)
                return false;

            var cleaned = CleanEngine.Clean(selectedText);
            if (!await TrySetTextAsync(cleaned))
                return false;

            if (targetWindow != IntPtr.Zero)
            {
                SetForegroundWindow(targetWindow);
                await Task.Delay(50);
            }

            SendKeys.SendWait("^v");
            await Task.Delay(PasteWaitMs);
            return true;
        }
        finally
        {
            if (backup is not null)
                await TrySetClipboardDataAsync(backup);
        }
    }

    public static async Task<bool> CleanClipboardAsync()
    {
        var text = await TryGetTextAsync();
        if (text is null)
            return false;

        return await TrySetTextAsync(CleanEngine.Clean(text));
    }

    public static IntPtr GetForegroundTarget() => GetForegroundWindow();

    private static async Task<string?> TryGetTextAsync()
    {
        for (var attempt = 0; attempt < ClipboardRetries; attempt++)
        {
            try
            {
                return Clipboard.ContainsText()
                    ? Clipboard.GetText(TextDataFormat.UnicodeText)
                    : null;
            }
            catch (ExternalException) when (attempt + 1 < ClipboardRetries)
            {
                await Task.Delay(RetryDelayMs);
            }
        }

        return null;
    }

    private static async Task<IDataObject?> TryGetClipboardDataAsync()
    {
        for (var attempt = 0; attempt < ClipboardRetries; attempt++)
        {
            try
            {
                return Clipboard.GetDataObject();
            }
            catch (ExternalException) when (attempt + 1 < ClipboardRetries)
            {
                await Task.Delay(RetryDelayMs);
            }
        }

        return null;
    }

    private static async Task<bool> TrySetTextAsync(string text)
    {
        for (var attempt = 0; attempt < ClipboardRetries; attempt++)
        {
            try
            {
                Clipboard.SetText(text, TextDataFormat.UnicodeText);
                return true;
            }
            catch (ExternalException) when (attempt + 1 < ClipboardRetries)
            {
                await Task.Delay(RetryDelayMs);
            }
        }

        return false;
    }

    private static async Task TryClearClipboardAsync()
    {
        for (var attempt = 0; attempt < ClipboardRetries; attempt++)
        {
            try
            {
                Clipboard.Clear();
                return;
            }
            catch (ExternalException) when (attempt + 1 < ClipboardRetries)
            {
                await Task.Delay(RetryDelayMs);
            }
        }
    }

    private static async Task TrySetClipboardDataAsync(IDataObject data)
    {
        for (var attempt = 0; attempt < ClipboardRetries; attempt++)
        {
            try
            {
                Clipboard.SetDataObject(data, copy: true);
                return;
            }
            catch (ExternalException) when (attempt + 1 < ClipboardRetries)
            {
                await Task.Delay(RetryDelayMs);
            }
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
}
