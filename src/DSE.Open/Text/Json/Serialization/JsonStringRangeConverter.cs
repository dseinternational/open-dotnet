// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="Range{T}"/> values as JSON strings.
/// </summary>
public class JsonStringRangeConverter<T> : SpanParsableCharWritingJsonConverter<Range<T>>
    where T : INumber<T>, IMinMaxValue<T>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringRangeConverter<T> Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(Range<T> value)
    {
        return Range<T>.MaxLength;
    }
}
