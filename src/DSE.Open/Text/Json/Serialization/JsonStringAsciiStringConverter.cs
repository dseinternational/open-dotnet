// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="AsciiString"/> values as JSON strings.
/// </summary>
public sealed class JsonStringAsciiStringConverter : SpanParsableByteWritingJsonConverter<AsciiString>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringAsciiStringConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxByteCountToWrite(AsciiString value)
    {
        return value.Length;
    }
}
