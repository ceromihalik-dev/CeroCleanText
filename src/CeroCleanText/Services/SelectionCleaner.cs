using CeroCleanText.Core;

namespace CeroCleanText.Services;

internal static class SelectionCleaner
{
    public static async Task<bool> CleanSelectionAsync()
    {
        SendKeys.SendWait("^c");
        await Task.Delay(80);

        if (!Clipboard.ContainsText())
            return false;

        var original = Clipboard.GetText(TextDataFormat.UnicodeText);
        var cleaned = CleanEngine.Clean(original);
        Clipboard.SetText(cleaned, TextDataFormat.UnicodeText);
        SendKeys.SendWait("^v");
        return true;
    }

    public static bool CleanClipboard()
    {
        if (!Clipboard.ContainsText())
            return false;

        var cleaned = CleanEngine.Clean(Clipboard.GetText(TextDataFormat.UnicodeText));
        Clipboard.SetText(cleaned, TextDataFormat.UnicodeText);
        return true;
    }
}
