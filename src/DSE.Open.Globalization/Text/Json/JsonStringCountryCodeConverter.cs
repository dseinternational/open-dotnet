// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Globalization.Text.Json;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that serializes
/// <see cref="CountryCode"/> values as JSON strings.
/// </summary>
public class JsonStringCountryCodeConverter : SpanParsableCharWritingJsonConverter<CountryCode>
{
    /// <summary>
    /// Gets the default shared instance of the <see cref="JsonStringCountryCodeConverter"/>.
    /// </summary>
    public static readonly JsonStringCountryCodeConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(CountryCode value)
    {
        return CountryCode.Length;
    }
}
