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
        ClientSize = new Size(390, 150);

        var title = new Label
        {
            Text = "CeroCleanText 0.1.0",
            AutoSize = true,
            Font = new Font(Font, FontStyle.Bold),
            Location = new Point(20, 18)
        };

        _startup = new CheckBox
        {
            Text = "CeroCleanText mit Windows starten",
            AutoSize = true,
            Checked = StartupService.IsEnabled(),
            Location = new Point(20, 55)
        };

        var hotkey = new Label
        {
            Text = "Globaler Hotkey: Ctrl + Alt + T",
            AutoSize = true,
            Location = new Point(20, 85)
        };

        var close = new Button
        {
            Text = "Schließen",
            DialogResult = DialogResult.OK,
            Size = new Size(90, 30),
            Location = new Point(280, 105)
        };

        _startup.CheckedChanged += (_, _) => StartupService.SetEnabled(_startup.Checked);

        Controls.AddRange([title, _startup, hotkey, close]);
        AcceptButton = close;
    }
}
