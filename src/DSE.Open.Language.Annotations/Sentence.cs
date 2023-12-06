// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

public sealed record Sentence
{
    [JsonPropertyName("sent_id")]
    public string? Id { get; init; } = string.Empty;

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("tokens")]
    public required ReadOnlyValueCollection<Word> Tokens { get; init; } = [];
}
