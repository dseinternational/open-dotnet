// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Speech.Abstractions;

/// <summary>
/// Defines a symbol used to represent a phoneme.
/// </summary>
public record PhoneticSymbol
{
    /// <summary>
    /// The phoneme represented.
    /// </summary>
    [JsonPropertyName("phoneme")]
    public required Phoneme Phoneme { get; init; }

    /// <summary>
    /// The symbol used to represent the phoneme.
    /// </summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>
    /// A collection of example words. The indicative parts should be surrounded
    /// with underscores - for example, "_c_a_k_e" and "du_ck_" for /k/.
    /// </summary>
    [JsonPropertyName("examples")]
    public ReadOnlyValueCollection<string> Examples { get; init; } = ReadOnlyValueCollection<string>.Empty;    
}
