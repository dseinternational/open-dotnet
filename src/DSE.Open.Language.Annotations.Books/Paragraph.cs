// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books;

public sealed record Paragraph
{
    [JsonPropertyName("para_id")]
    public string? Id { get; init; } = string.Empty;

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("sentences")]
    public required ReadOnlyValueCollection<Sentence> Sentences { get; init; } = [];

    [JsonIgnore]
    public IEnumerable<Word> Words => Sentences.SelectMany(t => t.Words);
}
