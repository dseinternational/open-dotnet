// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

/// <summary>
/// A source representation of a single page within a <see cref="BookSource"/>, composed of one or more <see cref="ParagraphSource"/>s.
/// </summary>
public sealed record PageSource
{
    /// <summary>
    /// Initializes a new <see cref="PageSource"/> with an empty <see cref="Paragraphs"/> collection.
    /// </summary>
    public PageSource()
    {
        Paragraphs = [];
    }

    /// <summary>
    /// Initializes a new <see cref="PageSource"/> with the specified paragraphs.
    /// </summary>
    /// <param name="paragraphs">The paragraphs on the page, in order.</param>
    [SetsRequiredMembers]
    public PageSource(IEnumerable<ParagraphSource> paragraphs)
    {
        Paragraphs = [.. paragraphs];
    }

    /// <summary>
    /// An identifier for the page.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The language of the page, if it differs from the containing <see cref="BookSource"/>.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The paragraphs on the page, in order.
    /// </summary>
    [JsonPropertyName("paragraphs")]
    public required ReadOnlyValueCollection<ParagraphSource> Paragraphs { get; init; }
}
