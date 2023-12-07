// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;
using System.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Books;

public sealed record Book
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("pages")]
    public required ReadOnlyValueCollection<Page> Pages { get; init; } = [];

    [JsonIgnore]
    public IEnumerable<Word> Words => Pages.SelectMany(p => p.Words);

    [JsonIgnore]
    public IEnumerable<Sentence> Sentences => Pages.SelectMany(p => p.Sentences);

    [JsonIgnore]
    public IEnumerable<Paragraph> Paragraphs => Pages.SelectMany(p => p.Paragraphs);
}
