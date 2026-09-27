using CeroCleanText.Services;

namespace CeroCleanText.UI;

internal sealed class HotkeyDialog : Form
{
    private readonly Label _display;
    private uint _modifiers;
    private uint _key;

    public uint Modifiers => _modifiers;
    public uint KeyCode => _key;

    public HotkeyDialog(uint currentModifiers, uint currentKey)
    {
        Text = "Hotkey ändern";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        KeyPreview = true;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(430, 220);
        Font = new Font("Segoe UI", 10F);
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application;

        _modifiers = currentModifiers;
        _key = currentKey;

        var title = new Label
        {
            Text = "Neue Tastenkombination drücken",
            AutoSize = true,
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            Location = new Point(28, 25)
        };

        var help = new Label
        {
            Text = "Verwenden Sie mindestens Strg oder Alt zusammen mit einer weiteren Taste.",
            AutoSize = false,
            Size = new Size(370, 42),
            ForeColor = SystemColors.GrayText,
            Location = new Point(28, 62)
        };

        _display = new Label
        {
            Text = HotkeyFormatter.Format(_modifiers, _key),
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            Location = new Point(28, 108),
            Size = new Size(374, 42)
        };

        var save = new Button
        {
            Text = "Übernehmen",
            DialogResult = DialogResult.OK,
            Location = new Point(190, 170),
            Size = new Size(100, 32)
        };
        var cancel = new Button
        {
            Text = "Abbrechen",
            DialogResult = DialogResult.Cancel,
            Location = new Point(302, 170),
            Size = new Size(100, 32)
        };

        Controls.AddRange([title, help, _display, save, cancel]);
        AcceptButton = save;
        CancelButton = cancel;
        KeyDown += CaptureHotkey;
    }

    private void CaptureHotkey(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.ControlKey or Keys.Menu or Keys.ShiftKey)
            return;

        var modifiers = 0u;
        if (e.Control) modifiers |= 0x0002;
        if (e.Alt) modifiers |= 0x0001;
        if (e.Shift) modifiers |= 0x0004;

        if ((modifiers & 0x0003) == 0)
            return;

        _modifiers = modifiers;
        _key = (uint)e.KeyCode;
        _display.Text = HotkeyFormatter.Format(_modifiers, _key);
        e.SuppressKeyPress = true;
    }
}

internal static class HotkeyFormatter
{
    public static string Format(uint modifiers, uint key)
    {
        var parts = new List<string>();
        if ((modifiers & 0x0002) != 0) parts.Add("Ctrl");
        if ((modifiers & 0x0001) != 0) parts.Add("Alt");
        if ((modifiers & 0x0004) != 0) parts.Add("Shift");
        parts.Add(((Keys)key).ToString());
        return string.Join(" + ", parts);
    }
}
