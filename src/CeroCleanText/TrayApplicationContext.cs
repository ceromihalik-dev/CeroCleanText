using CeroCleanText.Services;
using CeroCleanText.UI;

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
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application,
            ContextMenuStrip = menu,
            Visible = true
        };

        _trayIcon.MouseDown += (_, e) =>
        {
            if (e.Button == MouseButtons.Right)
                _lastTargetWindow = _foregroundTracker.LastExternalWindow;
        };
        menu.Items.Add("🧹 Jetzt bereinigen    Ctrl+Alt+T", null, async (_, _) => await CleanSelectionAsync(_lastTargetWindow));
        menu.Items.Add("📋 Zwischenablage bereinigen", null, async (_, _) => await SelectionCleaner.CleanClipboardAsync());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(_startupItem);
        menu.Items.Add("⚙ Einstellungen ...", null, (_, _) => ShowSettings());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("ⓘ Über CeroCleanText ...", null, (_, _) => ShowAbout());
        menu.Items.Add("❤️ Projekt unterstützen ...", null, (_, _) => OpenDonationPage());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Beenden", null, (_, _) => ExitThread());

        _hotkey = new HotkeyWindow();
        _hotkey.Pressed += async (_, _) =>
        {
            var target = SelectionCleaner.GetForegroundTarget();
            await CleanSelectionAsync(target, waitForHotkeyRelease: true);
        };
    }

    private async Task CleanSelectionAsync(IntPtr targetWindow, bool waitForHotkeyRelease = false)
    {
        try
        {
            var result = await SelectionCleaner.CleanSelectionAsync(targetWindow, waitForHotkeyRelease);
            if (result.Success && result.Changed && UserSettingsService.ShowCleanNotification)
            {
                _trayIcon.BalloonTipTitle = "Text bereinigt";
                _trayIcon.BalloonTipText = "Der markierte Text wurde erfolgreich bereinigt.";
                _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                _trayIcon.ShowBalloonTip(1800);
            }
        }
        catch
        {
            // Keep the tray process alive if clipboard/input access is temporarily unavailable.
        }
    }

    private void ShowSettings()
    {
        using var form = new SettingsForm();
        form.ShowDialog();
        _startupItem.Checked = StartupService.IsEnabled();
    }

    private static void ShowAbout()
    {
        using var form = new AboutForm();
        form.ShowDialog();
    }

    private static void OpenDonationPage()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.paypal.com/donate/?hosted_button_id=Y39Q96VMSJWG2",
                UseShellExecute = true
            });
        }
        catch
        {
            // Donation is optional; a browser launch failure must never affect the app.
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
