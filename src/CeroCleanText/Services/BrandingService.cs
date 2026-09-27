namespace CeroCleanText.Services;

internal static class BrandingService
{
    private static readonly Lazy<Icon> AppIconLazy = new(() =>
        Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? (Icon)SystemIcons.Application.Clone());

    public static Icon AppIcon => AppIconLazy.Value;

    public static Bitmap CreateLogoBitmap() => AppIcon.ToBitmap();
}
