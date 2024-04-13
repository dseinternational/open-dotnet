// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Speech;

/// <summary>
/// Defines a label used to represent a phoneme, together with example words.
/// </summary>
[Obsolete("To be removed.")]
public record PhonemeSymbol
{
    /// <summary>
    /// The phoneme represented.
    /// </summary>
    [JsonPropertyName("phoneme")]
    public required Phoneme Phoneme { get; init; }

    /// <summary>
    /// The symbol used to represent the phoneme - usually a letter or digraph in the language.
    /// </summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>
    /// The language the <see cref="Description"/> and <see cref="Examples"/> are provided in.
    /// </summary>
    [JsonPropertyName("language")]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// The (very) approximate order in which phoneme production is learned where the phoneme
    /// occurs at the start of the word.
    /// </summary>
    [JsonPropertyName("sequence_initial")]
    public required int SequenceInitial { get; init; }

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
    public ReadOnlyValueCollection<string> Examples { get; init; } = [];
}
