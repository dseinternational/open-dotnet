// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books;

/// <summary>
/// A single page within a <see cref="Book"/>, composed of one or more <see cref="Paragraph"/>s.
/// </summary>
public sealed record Page
{
    /// <summary>
    /// An identifier for the page.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The language of the page, if it differs from the containing <see cref="Book"/>.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The paragraphs on the page, in order.
    /// </summary>
    [JsonPropertyName("paragraphs")]
    public required ReadOnlyValueCollection<Paragraph> Paragraphs { get; init; } = [];

    /// <summary>
    /// Enumerates every <see cref="Word"/> across all paragraphs on the page.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Word> Words => Paragraphs.SelectMany(p => p.Words);

    /// <summary>
    /// Enumerates every <see cref="Sentence"/> across all paragraphs on the page.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Sentence> Sentences => Paragraphs.SelectMany(p => p.Sentences);
}
