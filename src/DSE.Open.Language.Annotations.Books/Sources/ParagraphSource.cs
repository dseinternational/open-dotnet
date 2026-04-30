// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Books.Sources;

/// <summary>
/// A source representation of a paragraph within a <see cref="PageSource"/>.
/// </summary>
public sealed record ParagraphSource
{
    /// <summary>
    /// Initializes a new <see cref="ParagraphSource"/>.
    /// </summary>
    public ParagraphSource()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ParagraphSource"/> with the specified text.
    /// </summary>
    /// <param name="text">The text of the paragraph.</param>
    [SetsRequiredMembers]
    public ParagraphSource(string text)
    {
        Text = text;
    }

    /// <summary>
    /// An identifier for the paragraph.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The language of the paragraph, if it differs from the containing <see cref="PageSource"/>.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The text of the paragraph.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}
