// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Requests;

/// <summary>
/// A <see cref="Request"/> that specifies the languages in which textual content
/// is to be interpreted and culture-sensitive values formatted.
/// </summary>
public record LocalizedRequest : Request, ILocalized
{
    /// <summary>
    /// The language used for textual content — for example, determining which
    /// resource translations to select for lookups triggered by this request.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonPropertyOrder(-899800)]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// The language used for formatting culture-sensitive values such as numbers,
    /// dates, and currencies.
    /// </summary>
    [JsonPropertyName("format_language")]
    [JsonPropertyOrder(-899700)]
    public required LanguageTag FormatLanguage { get; init; }
}
