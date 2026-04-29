// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

/// <summary>
/// A source representation of a book, prior to annotation, composed of an ordered sequence of <see cref="PageSource"/>s.
/// </summary>
public sealed record BookSource
{
    /// <summary>
    /// Initializes a new <see cref="BookSource"/> with an empty <see cref="Pages"/> collection.
    /// </summary>
    public BookSource()
    {
        Pages = [];
    }

    /// <summary>
    /// Initializes a new <see cref="BookSource"/> with the specified language, title and pages.
    /// </summary>
    /// <param name="language">The primary language of the book.</param>
    /// <param name="title">The title of the book.</param>
    /// <param name="pages">The pages of the book, in order.</param>
    [SetsRequiredMembers]
    public BookSource(
        LanguageTag language,
        string title,
        IEnumerable<PageSource> pages)
    {
        Language = language;
        Title = title;
        Pages = [.. pages];
    }

    /// <summary>
    /// An identifier for the book.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The primary language of the book.
    /// </summary>
    [JsonPropertyName("language")]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// The title of the book.
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    /// <summary>
    /// The pages of the book, in order.
    /// </summary>
    [JsonPropertyName("pages")]
    public required ReadOnlyValueCollection<PageSource> Pages { get; init; }
}
