// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Language;

/// <summary>
/// Associates a linguistic sign with a meaning.
/// </summary>
public record WordMeaning
{
    /// <summary>
    /// The sign with the meaning.
    /// </summary>
    [JsonPropertyName("sign")]
    [JsonPropertyOrder(-10000)]
    public required Sign Sign { get; init; }

    /// <summary>
    /// The language in which the word is presented.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonPropertyOrder(-8900)]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// A Universal POS tag for the word used in a context with the intended meaning.
    /// </summary>
    [JsonPropertyName("universal_pos_tag")]
    [JsonPropertyOrder(-8850)]
    public required UniversalPosTag PosTag { get; init; }

    /// <summary>
    /// A Treebank POS tag for the word used in a context with the intended meaning.
    /// </summary>
    [JsonPropertyName("treebank_pos_tag")]
    [JsonPropertyOrder(-8840)]
    public required TreebankPosTag PosDetailedTag { get; init; }
}
