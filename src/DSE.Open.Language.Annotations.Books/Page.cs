// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books;

public sealed record Page
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("paragraphs")]
    public required ReadOnlyValueCollection<Paragraph> Paragraphs { get; init; } = [];

    [JsonIgnore]
    public IEnumerable<Word> Words => Paragraphs.SelectMany(p => p.Words);

    [JsonIgnore]
    public IEnumerable<Sentence> Sentences => Paragraphs.SelectMany(p => p.Sentences);
}
