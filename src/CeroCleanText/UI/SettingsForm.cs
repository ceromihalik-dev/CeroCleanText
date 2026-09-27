using CeroCleanText.Services;

namespace CeroCleanText.UI;

internal sealed class SettingsForm : Form
{
    private readonly CheckBox _startup;

    public SettingsForm()
    {
        Text = "CeroCleanText – Einstellungen";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(510, 430);
        Font = new Font("Segoe UI", 10F);

        var header = new Label
        {
            Text = "CeroCleanText – Einstellungen",
            AutoSize = true,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            Location = new Point(28, 24)
        };

        var general = SectionLabel("Allgemein", 28, 78);
        _startup = new CheckBox
        {
            Text = "Mit Windows starten (Autostart)",
            AutoSize = true,
            Checked = StartupService.IsEnabled(),
            Location = new Point(30, 120),
            Font = new Font("Segoe UI", 10.5F)
        };
        var startupHelp = HelpLabel(
            "CeroCleanText wird automatisch mit Windows gestartet\r\nund im Infobereich (Tray) ausgeführt.",
            54, 151);

        var hotkeyTitle = SectionLabel("Hotkey", 28, 215);
        var hotkeyBox = new TextBox
        {
            Text = "Ctrl + Alt + T",
            ReadOnly = true,
            Location = new Point(30, 255),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold)
        };
        var hotkeyHelp = HelpLabel(
            "Mit diesem Hotkey wird der aktuell markierte Text\nbereinigt und wieder eingesetzt.",
            30, 296);

        var ok = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Size = new Size(130, 38),
            Location = new Point(215, 365)
        };
        var cancel = new Button
        {
            Text = "Abbrechen",
            DialogResult = DialogResult.Cancel,
            Size = new Size(130, 38),
            Location = new Point(355, 365)
        };

        _startup.CheckedChanged += (_, _) => StartupService.SetEnabled(_startup.Checked);

        Controls.AddRange([header, general, _startup, startupHelp, hotkeyTitle, hotkeyBox, hotkeyHelp, ok, cancel]);
        AcceptButton = ok;
        CancelButton = cancel;
    }

    private static Label SectionLabel(string text, int x, int y) => new()
    {
        Text = text,
        AutoSize = true,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
        Location = new Point(x, y)
    };

    private static Label HelpLabel(string text, int x, int y) => new()
    {
        Text = text,
        AutoSize = true,
        ForeColor = SystemColors.GrayText,
        Location = new Point(x, y)
    };
}
