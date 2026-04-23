// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books;

/// <summary>
/// A paragraph within a <see cref="Page"/>: a contiguous block of text composed of
/// one or more <see cref="Sentence"/>s.
/// </summary>
public sealed record Paragraph
{
    /// <summary>
    /// An identifier for the paragraph.
    /// </summary>
    [JsonPropertyName("para_id")]
    public string? Id { get; init; } = string.Empty;

    /// <summary>
    /// The language of the paragraph, if it differs from the containing <see cref="Page"/>.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The full text of the paragraph.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// The sentences making up the paragraph, in order.
    /// </summary>
    [JsonPropertyName("sentences")]
    public required ReadOnlyValueCollection<Sentence> Sentences { get; init; } = [];

    /// <summary>
    /// Enumerates every <see cref="Word"/> across all sentences in the paragraph.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Word> Words => Sentences.SelectMany(t => t.Words);
}
