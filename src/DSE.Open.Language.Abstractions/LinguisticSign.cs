// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Language;

/// <summary>
/// A sign that expresses a meaning in a particular language.
/// </summary>
public record LinguisticSign
{
    /// <summary>
    /// The language.
    /// </summary>
    [JsonPropertyName("language")]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// The sign as expressed in the <see cref="Language"/>.
    /// </summary>
    [JsonPropertyName("sign")]
    public required Sign Sign { get; init; }
}
