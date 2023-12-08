// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Stanza;

namespace DSE.Open.Language.Readability;

[Collection(nameof(StanzaContextCollection))]
public class SpacheReadabilityCalculatorTests : StanzaContextTestsBase
{
    public SpacheReadabilityCalculatorTests(StanzaContextFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Theory]
    [InlineData(2, 30, 6, 4.114)]
    [InlineData(2, 30, 0, 2.474)]
    public void CalculateLevel(int sentenceCount, int wordCount, int unfamiliarWordCount, double expected)
    {
        var result = SpacheReadabilityCalculator.CalculateLevel(sentenceCount, wordCount, unfamiliarWordCount);
        Assert.Equal(expected, result, 3);
    }

    // TODO: using books
}
