// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Globalization;

namespace DSE.Open.Requests;

public record LocalizedRequest : Request, ILocalized
{
    [JsonPropertyName("language")]
    [JsonPropertyOrder(-899800)]
    public required LanguageTag Language { get; init; }

    [JsonPropertyName("format_language")]
    [JsonPropertyOrder(-899700)]
    public required LanguageTag FormatLanguage { get; init; }
}
