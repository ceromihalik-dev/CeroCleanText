namespace CeroCleanText.Core;

public sealed record CleanOptions(
    bool RemoveZeroWidth = true,
    bool NormalizeWhitespace = true,
    bool RemoveControlCharacters = true,
    bool NormalizeLineEndings = true);
