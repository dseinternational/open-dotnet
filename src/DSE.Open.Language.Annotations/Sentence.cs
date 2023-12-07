// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

public sealed record Sentence
{
    private ReadOnlyValueCollection<Word>? _words;

    [JsonPropertyName("index")]
    public int Index { get; init; } = 1;

    [JsonPropertyName("sent_id")]
    public string? Id { get; init; } = string.Empty;

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("tokens")]
    public required ReadOnlyValueCollection<Token> Tokens { get; init; } = [];

    [JsonIgnore]
    public ReadOnlyValueCollection<Word> Words => _words ??= [.. Tokens.SelectMany(t => t.Words)];
}
