// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources.IO;

/// <summary>
/// Reads a <see cref="BookSource"/> from a Markdown text input, treating a level-1
/// heading as the book title and <c>---</c> lines as page breaks.
/// </summary>
public class BookSourceMarkdownTextReader
{
    /// <summary>
    /// Reads a <see cref="BookSource"/> from the Markdown file at the specified path.
    /// </summary>
    /// <param name="path">The path of the Markdown file to read.</param>
    /// <param name="id">The identifier to assign to the resulting <see cref="BookSource"/>.</param>
    /// <returns>The <see cref="BookSource"/> parsed from the file.</returns>
    public async Task<BookSource> ReadAsync(string path, string id)
    {
        using var stream = File.OpenRead(path);
        return await ReadAsync(stream, id).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a <see cref="BookSource"/> from the supplied Markdown <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream containing Markdown content to read.</param>
    /// <param name="id">The identifier to assign to the resulting <see cref="BookSource"/>.</param>
    /// <returns>The <see cref="BookSource"/> parsed from the stream.</returns>
    public async Task<BookSource> ReadAsync(Stream stream, string id)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var reader = new StreamReader(stream);
        return await ReadAsync(reader, id).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a <see cref="BookSource"/> from the supplied Markdown <see cref="StreamReader"/>.
    /// </summary>
    /// <param name="reader">The reader providing Markdown content.</param>
    /// <param name="id">The identifier to assign to the resulting <see cref="BookSource"/>.</param>
    /// <returns>The <see cref="BookSource"/> parsed from the reader.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public async Task<BookSource> ReadAsync(StreamReader reader, string id)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        string? title = null;
        var pages = new List<PageSource>();
        var paragraphs = new List<ParagraphSource>();

        string? line;

        while (null != (line = await reader.ReadLineAsync().ConfigureAwait(false)))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (pages.Count == 0 && title is null && LineIsHeading1(line))
            {
                title = GetTextFromHeadingLine(line);
                paragraphs.Add(new(title));
                pages.Add(new(paragraphs));
                paragraphs.Clear();
            }
            else if (LineIsPageBreak(line))
            {
                pages.Add(new(paragraphs));
                paragraphs.Clear();
            }
            else
            {
                paragraphs.Add(new(line));
            }
        }

        if (paragraphs.Count > 0)
        {
            pages.Add(new(paragraphs));
        }

        return new(LanguageTag.EnglishUk, title ?? string.Empty, pages)
        {
            Id = id
        };
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
