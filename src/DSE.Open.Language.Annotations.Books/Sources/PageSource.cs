// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

public sealed record PageSource
{
    public PageSource()
    {
        Paragraphs = [];
    }

    [SetsRequiredMembers]
    public PageSource(IEnumerable<ParagraphSource> paragraphs)
    {
        Paragraphs = [.. paragraphs];
    }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("paragraphs")]
    public required ReadOnlyValueCollection<ParagraphSource> Paragraphs { get; init; }
}
