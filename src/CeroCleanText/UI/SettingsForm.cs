using CeroCleanText.Services;

namespace CeroCleanText.UI;

internal sealed class SettingsForm : Form
{
    private readonly Panel _content = new();
    private readonly CheckBox _startup;

    public SettingsForm()
    {
        Text = "CeroCleanText – Einstellungen";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(720, 430);
        Font = new Font("Segoe UI", 10F);

        var nav = new Panel { Dock = DockStyle.Left, Width = 175, BackColor = Color.FromArgb(245, 248, 252) };
        _content.Dock = DockStyle.Fill;

        var generalButton = NavButton("⚙  Allgemein", 28);
        var hotkeyButton = NavButton("⌨  Hotkey", 78);
        var infoButton = NavButton("ⓘ  Info", 128);
        generalButton.Click += (_, _) => ShowGeneral();
        hotkeyButton.Click += (_, _) => ShowHotkey();
        infoButton.Click += (_, _) => ShowInfo();
        nav.Controls.AddRange([generalButton, hotkeyButton, infoButton]);

        _startup = new CheckBox
        {
            Text = "Mit Windows starten (Autostart)",
            AutoSize = true,
            Checked = StartupService.IsEnabled(),
            Font = new Font("Segoe UI", 10.5F)
        };
        _startup.CheckedChanged += (_, _) => StartupService.SetEnabled(_startup.Checked);

        Controls.Add(_content);
        Controls.Add(nav);
        ShowGeneral();
    }

    private void ShowGeneral()
    {
        PreparePage("Allgemein");
        _startup.Location = new Point(32, 80);
        _content.Controls.Add(_startup);
        _content.Controls.Add(Help(
            "CeroCleanText wird automatisch mit Windows gestartet und im Infobereich (Tray) ausgeführt.\r\n" +
            "Es wird kein Hauptfenster dauerhaft geöffnet.",
            32, 118));
    }

    private void ShowHotkey()
    {
        PreparePage("Hotkey");
        var box = new TextBox
        {
            Text = "Ctrl + Alt + T",
            ReadOnly = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            Location = new Point(32, 80),
            Size = new Size(255, 30)
        };
        var change = new Button
        {
            Text = "Ändern ...",
            Enabled = false,
            Location = new Point(305, 78),
            Size = new Size(120, 34)
        };
        _content.Controls.AddRange([
            box,
            change,
            Help("Mit diesem Hotkey wird der aktuell markierte Text bereinigt und wieder eingesetzt.\r\nDie freie Hotkey-Wahl folgt nach der 0.1.0-Grundabnahme.", 32, 128)
        ]);
    }

    private void ShowInfo()
    {
        PreparePage("Info");
        _content.Controls.Add(Help(
            "CeroCleanText 0.1.0\r\nSauberer Text – ganz automatisch.\r\n\r\nTextbereinigung erfolgt vollständig lokal.\r\nEntwickler: C. Mihalik",
            32, 82));
    }

    private void PreparePage(string title)
    {
        _content.Controls.Clear();
        _content.Controls.Add(new Label
        {
            Text = title,
            AutoSize = true,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Location = new Point(30, 25)
        });
    }

    private static Button NavButton(string text, int y) => new()
    {
        Text = text,
        TextAlign = ContentAlignment.MiddleLeft,
        FlatStyle = FlatStyle.Flat,
        FlatAppearance = { BorderSize = 0 },
        Location = new Point(12, y),
        Size = new Size(150, 42),
        Font = new Font("Segoe UI", 10F)
    };

    private static Label Help(string text, int x, int y) => new()
    {
        Text = text,
        AutoSize = true,
        ForeColor = SystemColors.GrayText,
        Location = new Point(x, y)
    };
}
