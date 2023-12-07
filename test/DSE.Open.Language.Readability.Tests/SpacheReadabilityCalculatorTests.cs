// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Nlp.Stanza;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language.Readability;

[Collection(nameof(StanzaContextCollection))]
public class SpacheReadabilityCalculatorTests : LoggedTestsBase
{
    public SpacheReadabilityCalculatorTests(StanzaContextFixture fixture, ITestOutputHelper output) : base(output)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        PythonContext = fixture.PythonContext;
        StanzaContext = fixture.StanzaContext;
    }

    public PythonContext PythonContext { get; }

    public StanzaContext StanzaContext { get; }

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
