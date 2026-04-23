// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books;

/// <summary>
/// An annotated book composed of an ordered sequence of <see cref="Page"/>s.
/// </summary>
public sealed record Book
{
    /// <summary>
    /// An identifier for the book.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The primary language of the book, if known.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The pages of the book, in order.
    /// </summary>
    [JsonPropertyName("pages")]
    public required ReadOnlyValueCollection<Page> Pages { get; init; } = [];

    /// <summary>
    /// Enumerates every <see cref="Word"/> across all pages.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Word> Words => Pages.SelectMany(p => p.Words);

    /// <summary>
    /// Enumerates every <see cref="Sentence"/> across all pages.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Sentence> Sentences => Pages.SelectMany(p => p.Sentences);

    /// <summary>
    /// Enumerates every <see cref="Paragraph"/> across all pages.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Paragraph> Paragraphs => Pages.SelectMany(p => p.Paragraphs);
}
