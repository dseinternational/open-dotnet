// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

public sealed record ParagraphSource
{
    public ParagraphSource()
    {
    }

    [SetsRequiredMembers]
    public ParagraphSource(string text)
    {
        Text = text;
    }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }
}
