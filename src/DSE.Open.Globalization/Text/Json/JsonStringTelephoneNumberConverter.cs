// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Globalization.Text.Json;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that serializes
/// <see cref="TelephoneNumber"/> values as JSON strings.
/// </summary>
public class JsonStringTelephoneNumberConverter : SpanParsableCharWritingJsonConverter<TelephoneNumber>
{
    /// <summary>
    /// Gets the default shared instance of the <see cref="JsonStringTelephoneNumberConverter"/>.
    /// </summary>
    public static readonly JsonStringTelephoneNumberConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(TelephoneNumber value)
    {
        return TelephoneNumber.MaxFormattedLength;
    }
}
