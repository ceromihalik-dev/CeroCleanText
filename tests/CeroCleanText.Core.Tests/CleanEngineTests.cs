using CeroCleanText.Core;
using Xunit;

namespace CeroCleanText.Core.Tests;

public sealed class CleanEngineTests
{
    [Fact]
    public void RemovesZeroWidthCharacters()
        => Assert.Equal("CeroCleanText", CleanEngine.Clean("Cero\u200BClean\uFEFFText"));

    [Fact]
    public void NormalizesSpecialSpaces()
        => Assert.Equal("A B C", CleanEngine.Clean("A\u00A0B\u202FC"));

    [Fact]
    public void PreservesGermanCharacters()
        => Assert.Equal("ÄÖÜ äöü ß €", CleanEngine.Clean("ÄÖÜ äöü ß €"));

    [Fact]
    public void RemovesControlCharactersButKeepsTabsAndLines()
        => Assert.Equal($"A\tB{Environment.NewLine}C", CleanEngine.Clean("A\tB\n\u0007C"));

    [Fact]
    public void CanLeaveZeroWidthCharactersUntouched()
        => Assert.Equal("A\u200BB", CleanEngine.Clean("A\u200BB", new CleanOptions(RemoveZeroWidth: false)));
}
