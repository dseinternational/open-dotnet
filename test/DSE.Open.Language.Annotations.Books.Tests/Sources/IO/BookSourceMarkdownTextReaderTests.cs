// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using DSE.Open.Globalization;
using DSE.Open.Language.Annotations.Books.Sources.IO;

namespace DSE.Open.Language.Annotations.Books.Sources;

public class BookSourceMarkdownTextReaderTests
{
    [Fact]
    public async Task Read_TitleOnly_CreatesSinglePageWithTitleParagraph()
    {
        var book = await ReadAsync("# My Book\n");

        Assert.Equal(LanguageTag.EnglishUk, book.Language);
        Assert.Equal("My Book", book.Title);
        var page = Assert.Single(book.Pages);
        var paragraph = Assert.Single(page.Paragraphs);
        Assert.Equal("My Book", paragraph.Text);
    }

    [Fact]
    public async Task Read_TitleAndPages_SeparatesPagesOnPageBreak()
    {
        var book = await ReadAsync("""
            # My Book

            First page paragraph.

            ---

            Second page paragraph.

            ---

            Third page paragraph.
            """);

        Assert.Equal("My Book", book.Title);
        Assert.Equal(4, book.Pages.Count); // title page + 3 content pages

        Assert.Equal("My Book", book.Pages[0].Paragraphs.Single().Text);
        Assert.Equal("First page paragraph.", book.Pages[1].Paragraphs.Single().Text);
        Assert.Equal("Second page paragraph.", book.Pages[2].Paragraphs.Single().Text);
        Assert.Equal("Third page paragraph.", book.Pages[3].Paragraphs.Single().Text);
    }

    [Fact]
    public async Task Read_PageWithMultipleParagraphs_PreservesAllLines()
    {
        var book = await ReadAsync("""
            # My Book

            Paragraph one.
            Paragraph two.
            Paragraph three.

            ---

            Last page paragraph.
            """);

        Assert.Equal(3, book.Pages[1].Paragraphs.Count);
        Assert.Equal("Paragraph one.", book.Pages[1].Paragraphs[0].Text);
        Assert.Equal("Paragraph two.", book.Pages[1].Paragraphs[1].Text);
        Assert.Equal("Paragraph three.", book.Pages[1].Paragraphs[2].Text);
    }

    [Fact]
    public async Task Read_BlankLines_AreIgnored()
    {
        var book = await ReadAsync("""
            # My Book


            Only paragraph.

            """);

        var contentPage = book.Pages[1];
        var paragraph = Assert.Single(contentPage.Paragraphs);
        Assert.Equal("Only paragraph.", paragraph.Text);
    }

    [Fact]
    public async Task Read_NoTitleHeading_ReturnsEmptyTitle()
    {
        var book = await ReadAsync("Just a paragraph.\n");

        Assert.Equal(string.Empty, book.Title);
        var page = Assert.Single(book.Pages);
        Assert.Equal("Just a paragraph.", page.Paragraphs.Single().Text);
    }

    [Fact]
    public async Task Read_HashWithoutSpace_TreatedAsBodyParagraph()
    {
        // "# Heading" requires the space between '#' and the title text. Without it,
        // the line should not be treated as the title.
        var book = await ReadAsync("#NotATitle\n");

        Assert.Equal(string.Empty, book.Title);
        var paragraph = book.Pages.Single().Paragraphs.Single();
        Assert.Equal("#NotATitle", paragraph.Text);
    }

    private static async Task<BookSource> ReadAsync(string markdown)
    {
        var reader = new BookSourceMarkdownTextReader();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(markdown));
        return await reader.ReadAsync(stream, "test-id");
    }
}
