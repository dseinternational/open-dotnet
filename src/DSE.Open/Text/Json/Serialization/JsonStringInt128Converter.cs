// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="Int128"/> values as JSON strings.
/// </summary>
public class JsonStringInt128Converter : SpanParsableCharWritingJsonConverter<Int128>
{
    private const int MaxCharCount = 40;

    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringInt128Converter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(Int128 value)
    {
        return MaxCharCount;
    }
}
