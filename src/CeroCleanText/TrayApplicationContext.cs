using CeroCleanText.Services;

namespace CeroCleanText;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly HotkeyWindow _hotkey;
    private readonly ToolStripMenuItem _startupItem;
    private IntPtr _lastTargetWindow;

    public TrayApplicationContext()
    {
        _startupItem = new ToolStripMenuItem("Mit Windows starten")
        {
            Checked = StartupService.IsEnabled(),
            CheckOnClick = true
        };
        _startupItem.CheckedChanged += (_, _) => StartupService.SetEnabled(_startupItem.Checked);

        var menu = new ContextMenuStrip();
        menu.Opening += (_, _) => _lastTargetWindow = SelectionCleaner.GetForegroundTarget();
        menu.Items.Add("Markierten Text bereinigen", null, async (_, _) => await CleanSelectionAsync(_lastTargetWindow));
        menu.Items.Add("Zwischenablage bereinigen", null, async (_, _) => await SelectionCleaner.CleanClipboardAsync());
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
        _hotkey.Pressed += async (_, _) =>
        {
            var target = SelectionCleaner.GetForegroundTarget();
            await CleanSelectionAsync(target);
        };
    }

    private static async Task CleanSelectionAsync(IntPtr targetWindow)
    {
        try
        {
            var result = await SelectionCleaner.CleanSelectionAsync(targetWindow);
            MessageBox.Show(
                $"Stage: {result.Stage}\r\nSuccess: {result.Success}\r\nInput: {result.InputLength}\r\nOutput: {result.OutputLength}\r\nChanged: {result.Changed}",
                "CeroCleanText QA Diagnose",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"{ex.GetType().Name}: {ex.Message}",
                "CeroCleanText QA Fehler",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
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
