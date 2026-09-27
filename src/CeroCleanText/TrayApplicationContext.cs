using CeroCleanText.Services;

namespace CeroCleanText;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly HotkeyWindow _hotkey;
    private readonly ToolStripMenuItem _startupItem;

    public TrayApplicationContext()
    {
        _startupItem = new ToolStripMenuItem("Mit Windows starten")
        {
            Checked = StartupService.IsEnabled(),
            CheckOnClick = true
        };
        _startupItem.CheckedChanged += (_, _) => StartupService.SetEnabled(_startupItem.Checked);

        var menu = new ContextMenuStrip();
        menu.Items.Add("Markierten Text bereinigen", null, async (_, _) => await CleanSelectionAsync());
        menu.Items.Add("Zwischenablage bereinigen", null, (_, _) => SelectionCleaner.CleanClipboard());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(_startupItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Beenden", null, (_, _) => ExitThread());

        _trayIcon = new NotifyIcon
        {
            Text = "CeroCleanText 0.1.0",
            Icon = SystemIcons.Application,
            ContextMenuStrip = menu,
            Visible = true
        };

        _hotkey = new HotkeyWindow();
        _hotkey.Pressed += async (_, _) => await CleanSelectionAsync();
    }

    private async Task CleanSelectionAsync()
    {
        try
        {
            await SelectionCleaner.CleanSelectionAsync();
        }
        catch
        {
            // Keep the tray process alive if clipboard access is temporarily unavailable.
        }
    }

    protected override void ExitThreadCore()
    {
        _trayIcon.Visible = false;
        _trayIcon.Dispose();
        _hotkey.Dispose();
        base.ExitThreadCore();
    }
}
