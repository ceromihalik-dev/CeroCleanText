using System.Text;

namespace CeroCleanText.Core;

public static class CleanEngine
{
    private static readonly HashSet<Rune> ZeroWidth =
    [
        new(0x200B), new(0x200C), new(0x200D), new(0x2060), new(0xFEFF)
    ];

    private static readonly HashSet<Rune> SpecialSpaces =
    [
        new(0x00A0), new(0x1680), new(0x2000), new(0x2001), new(0x2002),
        new(0x2003), new(0x2004), new(0x2005), new(0x2006), new(0x2007),
        new(0x2008), new(0x2009), new(0x200A), new(0x202F), new(0x205F),
        new(0x3000)
    ];

    public static string Clean(string? input, CleanOptions? options = null)
    {
        if (string.IsNullOrEmpty(input))
            return input ?? string.Empty;

        options ??= new CleanOptions();
        var source = options.NormalizeLineEndings
            ? input.Replace("\r\n", "\n").Replace("\r", "\n")
            : input;

        var output = new StringBuilder(source.Length);

        foreach (var rune in source.EnumerateRunes())
        {
            if (options.RemoveZeroWidth && ZeroWidth.Contains(rune))
                continue;

            if (options.NormalizeWhitespace && SpecialSpaces.Contains(rune))
            {
                output.Append(' ');
                continue;
            }

            if (options.RemoveControlCharacters &&
                Rune.GetUnicodeCategory(rune) == System.Globalization.UnicodeCategory.Control &&
                rune.Value is not 0x0A and not 0x09)
                continue;

            output.Append(rune.ToString());
        }

        return options.NormalizeLineEndings
            ? output.ToString().Replace("\n", Environment.NewLine)
            : output.ToString();
    }
}
