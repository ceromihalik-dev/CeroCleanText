namespace CeroCleanText.UI;

internal sealed class AboutForm : Form
{
    public AboutForm()
    {
        Text = "Über CeroCleanText";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(430, 230);

        var title = new Label
        {
            Text = "CeroCleanText",
            AutoSize = true,
            Font = new Font(Font.FontFamily, 16, FontStyle.Bold),
            Location = new Point(20, 18)
        };

        var info = new Label
        {
            Text = "Version 0.1.0\r\n\r\nLightweight Windows text cleaner.\r\nText processing runs locally on this computer.",
            AutoSize = true,
            Location = new Point(22, 60)
        };

        var github = new LinkLabel
        {
            Text = "GitHub – CeroCleanText",
            AutoSize = true,
            Location = new Point(22, 140)
        };
        github.LinkClicked += (_, _) => OpenUrl("https://github.com/ceromihalik-dev/CeroCleanText");

        var support = new LinkLabel
        {
            Text = "❤️ Projekt unterstützen",
            AutoSize = true,
            Location = new Point(22, 170)
        };
        support.LinkClicked += (_, _) => OpenUrl("https://www.paypal.com/donate/?hosted_button_id=Y39Q96VMSJWG2");

        var close = new Button
        {
            Text = "Schließen",
            DialogResult = DialogResult.OK,
            Size = new Size(90, 30),
            Location = new Point(320, 185)
        };

        Controls.AddRange([title, info, github, support, close]);
        AcceptButton = close;
    }

    private static void OpenUrl(string url)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            // External links are optional and must not affect the application.
        }
    }
}
