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
        ClientSize = new Size(520, 430);
        Font = new Font("Segoe UI", 10F);

        var mark = new Label
        {
            Text = "C",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),
            ForeColor = SystemColors.Highlight,
            Location = new Point(28, 30),
            Size = new Size(70, 70)
        };

        var title = new Label
        {
            Text = "CeroCleanText",
            AutoSize = true,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            Location = new Point(115, 35)
        };
        var version = new Label
        {
            Text = "Sauberer Text – ganz automatisch.\r\nVersion 0.1.0",
            AutoSize = true,
            Font = new Font("Segoe UI", 12F),
            Location = new Point(116, 70)
        };
        var slogan = new Label
        {
            Text = "100 % lokal · keine Textdatenübertragung",
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(30, 125)
        };
        var privacy = new Label
        {
            Text = "CeroCleanText bereinigt markierten Text vollständig lokal auf Ihrem Computer.\r\n" +
                   "Es werden keine Textdaten an das Internet oder externe Dienste übertragen.",
            AutoSize = true,
            Location = new Point(30, 165)
        };

        var github = new LinkLabel
        {
            Text = "GitHub-Projekt öffnen",
            AutoSize = true,
            Location = new Point(55, 250),
            Font = new Font("Segoe UI", 10.5F)
        };
        github.LinkClicked += (_, _) => OpenUrl("https://github.com/ceromihalik-dev/CeroCleanText");

        var support = new LinkLabel
        {
            Text = "❤️  Projekt unterstützen (PayPal)",
            AutoSize = true,
            Location = new Point(55, 292),
            Font = new Font("Segoe UI", 10.5F)
        };
        support.LinkClicked += (_, _) => OpenUrl("https://www.paypal.com/donate/?hosted_button_id=Y39Q96VMSJWG2");

        var copyright = new Label
        {
            Text = "© 2026 C. Mihalik",
            AutoSize = true,
            ForeColor = SystemColors.GrayText,
            Location = new Point(30, 370)
        };

        var close = new Button
        {
            Text = "Schließen",
            DialogResult = DialogResult.OK,
            Size = new Size(120, 36),
            Location = new Point(370, 365)
        };

        Controls.AddRange([mark, title, version, slogan, privacy, github, support, copyright, close]);
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
