// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="CharSequence"/> values as JSON strings.
/// </summary>
public sealed class JsonStringCharSequenceConverter : SpanParsableCharWritingJsonConverter<CharSequence>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringCharSequenceConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(CharSequence value)
    {
        return value.Length;
    }
}
