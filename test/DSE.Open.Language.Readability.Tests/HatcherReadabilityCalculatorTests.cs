// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Nlp.Stanza;

namespace DSE.Open.Language.Readability;

[Collection(nameof(StanzaContextCollection))]
public class HatcherReadabilityCalculatorTests : StanzaContextTestsBase
{
    public HatcherReadabilityCalculatorTests(StanzaContextFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    /*
     * TODO
     * 
    [Theory]
    [MemberData(nameof(GetTestBooks))]
    public void CanScore(Book book, int maxLines, int syntaxComplexity, double expectedLevel)
    {
        var calculator = new HatcherReadabilityCalculator();
        var result = calculator.Calculate(book, maxLines, syntaxComplexity);
        Assert.Equal(result.MaxLinesOnPage, maxLines);
        Assert.Equal(expectedLevel, result.Level, 4);
    }

    public static IEnumerable<object[]> GetTestBooks()
    {
        yield return new object[] {
            new Book("test", "A test book", new List<Page>
            {
                new Page(new List<Paragraph>
                {
                    new Paragraph("Hello Tom!")
                }),
                new Page(new List<Paragraph>
                {
                    new Paragraph("Hello Tom!")
                }),
                new Page(new List<Paragraph>
                {
                    new Paragraph("Hello Tom!")
                })
            }),
            1, // max lines
            0, // complexity
            -1.630304 // expected score
        };

        yield return new object[] {
            new Book("test", "A test book", new List<Page>
            {
                new Page(new List<Paragraph>
                {
                    new Paragraph("Good morning Emma")
                }),
                new Page(new List<Paragraph>
                {
                    new Paragraph("Emma is in her bed. It is morning.")
                }),
                new Page(new List<Paragraph>
                {
                    new Paragraph("Emma gets up and goes to the kitchen.")
                }),
                new Page(new List<Paragraph>
                {
                    new Paragraph("Emma is eating cereal.")
                })
            }),
            2, // max lines
            1, // complexity
            2.161631 // expected score
        };
    }
    */
}
