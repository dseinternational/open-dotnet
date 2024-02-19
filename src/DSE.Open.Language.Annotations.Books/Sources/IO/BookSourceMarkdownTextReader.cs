// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources.IO;

public class BookSourceMarkdownTextReader
{
    public Task<BookSource> ReadAsync(string path, string id)
    {
        using var stream = File.OpenRead(path);
        return ReadAsync(stream, id);
    }

    public Task<BookSource> ReadAsync(Stream stream, string id)
    {
        Guard.IsNotNull(stream);

        using var reader = new StreamReader(stream);
        return ReadAsync(reader, id);
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public async Task<BookSource> ReadAsync(StreamReader reader, string id)
    {
        Guard.IsNotNull(reader);

        string? title = null;
        var pages = new List<PageSource>();
        var paragraphs = new List<ParagraphSource>();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (pages.Count == 0 && title is null && LineIsHeading1(line))
            {
                title = GetTextFromHeadingLine(line);
                paragraphs.Add(new ParagraphSource(title));
                pages.Add(new PageSource(paragraphs));
                paragraphs.Clear();
            }
            else if (LineIsPageBreak(line))
            {
                pages.Add(new PageSource(paragraphs));
                paragraphs.Clear();
            }
            else
            {
                paragraphs.Add(new ParagraphSource(line));
            }
        }

        if (paragraphs.Count > 0)
        {
            pages.Add(new PageSource(paragraphs));
        }

        return new BookSource(LanguageTag.EnglishUk, title ?? string.Empty, pages);
    }

    private static bool LineIsHeading1(string line)
    {
        return line.Length > 2
            && line[0] == '#'
            && char.IsWhiteSpace(line[1]);
    }

    private static string GetTextFromHeadingLine(string line)
    {
        var headingEnded = false;
        for (var i = 0; i < line.Length; i++)
        {
            if (!headingEnded)
            {
                if (char.IsWhiteSpace(line[i]))
                {
                    headingEnded = true;
                }
                else if (line[i] != '#')
                {
                    throw new InvalidOperationException("Invalid heading character.");
                }

                continue;
            }
            else
            {
                return line[i..];
            }
        }

        return string.Empty;
    }

    private static bool LineIsPageBreak(string line)
    {
        return line == "---";
    }
}
