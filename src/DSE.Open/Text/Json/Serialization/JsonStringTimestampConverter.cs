// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="Timestamp"/> values as JSON strings.
/// </summary>
public class JsonStringTimestampConverter : SpanParsableCharWritingJsonConverter<Timestamp>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringTimestampConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(Timestamp value)
    {
        return Timestamp.Base64Length;
    }
}
