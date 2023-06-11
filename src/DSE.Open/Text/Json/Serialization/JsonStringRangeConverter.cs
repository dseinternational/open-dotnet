// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringRangeConverter<T> : SpanParsableCharWritingJsonConverter<Range<T>>
    where T : INumber<T>
{
    public static readonly JsonStringRangeConverter<T> Default = new();

    protected override int GetMaxCharCountToWrite(Range<T> value) => Range<T>.MaxLength;
}
