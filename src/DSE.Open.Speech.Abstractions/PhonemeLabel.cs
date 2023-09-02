// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Speech.Abstractions;

/// <summary>
/// Defines a label used to represent a phoneme, together with example words.
/// </summary>
public record PhonemeLabel
{
    /// <summary>
    /// The phoneme represented.
    /// </summary>
    [JsonPropertyName("phoneme")]
    public required Phoneme Phoneme { get; init; }

    /// <summary>
    /// The label used to represent the phoneme.
    /// </summary>
    [JsonPropertyName("label")]
    public required string Label { get; init; }

    /// <summary>
    /// The language the <see cref="Description"/> and <see cref="Examples"/> are provided in.
    /// </summary>
    [JsonPropertyName("language")]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// An optional description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// A collection of example words. The indicative parts should be surrounded
    /// with underscores - for example, "_c_a_k_e" and "du_ck_" for /k/.
    /// </summary>
    [JsonPropertyName("examples")]
    public ReadOnlyValueCollection<string> Examples { get; init; } = ReadOnlyValueCollection<string>.Empty;
}
