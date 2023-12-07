// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;
using System.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Books.Sources;

public sealed record PageSource
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("paragraphs")]
    public required ReadOnlyValueCollection<ParagraphSource> Paragraphs { get; init; } = [];
}
