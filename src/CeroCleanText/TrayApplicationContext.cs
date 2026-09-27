using CeroCleanText.Services;

namespace CeroCleanText;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly HotkeyWindow _hotkey;
    private readonly ToolStripMenuItem _startupItem;
    private readonly ForegroundWindowTracker _foregroundTracker;
    private IntPtr _lastTargetWindow;

    public TrayApplicationContext()
    {
        _foregroundTracker = new ForegroundWindowTracker();
        _startupItem = new ToolStripMenuItem("Mit Windows starten")
        {
            Checked = StartupService.IsEnabled(),
            CheckOnClick = true
        };
        _startupItem.CheckedChanged += (_, _) => StartupService.SetEnabled(_startupItem.Checked);

        var menu = new ContextMenuStrip();
        _trayIcon = new NotifyIcon
        {
            Text = "CeroCleanText 0.1.0",
            Icon = SystemIcons.Application,
            ContextMenuStrip = menu,
            Visible = true
        };

        _trayIcon.MouseDown += (_, e) =>
        {
            if (e.Button == MouseButtons.Right)
                _lastTargetWindow = _foregroundTracker.LastExternalWindow;
        };
        menu.Items.Add("Markierten Text bereinigen", null, async (_, _) => await CleanSelectionAsync(_lastTargetWindow));
        menu.Items.Add("Zwischenablage bereinigen", null, async (_, _) => await SelectionCleaner.CleanClipboardAsync());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(_startupItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Beenden", null, (_, _) => ExitThread());

        _hotkey = new HotkeyWindow();
        _hotkey.Pressed += async (_, _) =>
        {
            var target = SelectionCleaner.GetForegroundTarget();
            await CleanSelectionAsync(target, waitForHotkeyRelease: true);
        };
    }

    private static async Task CleanSelectionAsync(IntPtr targetWindow, bool waitForHotkeyRelease = false)
    {
        try
        {
            await SelectionCleaner.CleanSelectionAsync(targetWindow, waitForHotkeyRelease);
        }
        catch
        {
            // Keep the tray process alive if clipboard/input access is temporarily unavailable.
        }
    }

    protected override void ExitThreadCore()
    {
        _trayIcon.Visible = false;
        _trayIcon.Dispose();
        _hotkey.Dispose();
        _foregroundTracker.Dispose();
        base.ExitThreadCore();
    }
}
