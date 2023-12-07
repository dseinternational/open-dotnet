// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

public sealed record BookSource
{
    public BookSource()
    {
        Pages = [];
    }

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

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public required LanguageTag Language { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("pages")]
    public required ReadOnlyValueCollection<PageSource> Pages { get; init; }
}
