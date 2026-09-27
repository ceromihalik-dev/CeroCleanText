using CeroCleanText.Services;

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
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(560, 440);
        Font = new Font("Segoe UI", 10F);
        Icon = BrandingService.AppIcon;

        var mark = new Label
        {
            Text = "",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 120, 215),
            Location = new Point(28, 30),
            Size = new Size(70, 70)
        };

        var title = new Label
        {
            Text = "CeroCleanText",
            AutoSize = true,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            Location = new Point(138, 34)
        };
        var version = new Label
        {
            Text = "Sauberer Text – ganz automatisch.\r\nVersion 0.1.0",
            AutoSize = true,
            Font = new Font("Segoe UI", 12F),
            Location = new Point(139, 70)
        };
        var slogan = new Label
        {
            Text = "100 % lokal · keine Textdatenübertragung",
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(30, 135)
        };
        var privacy = new Label
        {
            Text = "CeroCleanText bereinigt markierten Text vollständig lokal auf Ihrem Computer.\r\n" +
                   "Es werden keine Textdaten an das Internet oder externe Dienste übertragen.",
            AutoSize = true,
            Location = new Point(30, 185)
        };

        var github = new LinkLabel
        {
            Text = "GitHub-Projekt öffnen",
            AutoSize = true,
            Location = new Point(55, 270),
            Font = new Font("Segoe UI", 10.5F)
        };
        github.LinkClicked += (_, _) => OpenUrl("https://github.com/ceromihalik-dev/CeroCleanText");

        var support = new LinkLabel
        {
            Text = "❤️  Projekt unterstützen (PayPal)",
            AutoSize = true,
            Location = new Point(55, 310),
            Font = new Font("Segoe UI", 10.5F)
        };
        support.LinkClicked += (_, _) => OpenUrl("https://www.paypal.com/donate/?hosted_button_id=Y39Q96VMSJWG2");

        var copyright = new Label
        {
            Text = "© 2026 C. Mihalik",
            AutoSize = true,
            ForeColor = SystemColors.GrayText,
            Location = new Point(30, 395)
        };

        var close = new Button
        {
            Text = "Schließen",
            DialogResult = DialogResult.OK,
            Size = new Size(120, 36),
            Location = new Point(410, 385)
        };

                var logo = new PictureBox
        {
            Image = BrandingService.CreateLogoBitmap(),
            SizeMode = PictureBoxSizeMode.Zoom,
            Location = new Point(28, 28),
            Size = new Size(92, 92)
        };

        Controls.AddRange([logo, title, version, slogan, privacy, github, support, copyright, close]);
        FormClosed += (_, _) => logo.Image?.Dispose();
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
