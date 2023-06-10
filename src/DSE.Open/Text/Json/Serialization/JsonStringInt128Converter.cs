// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringInt128Converter : SpanParsableCharWritingJsonConverter<Int128>
{
    private const int MaxCharCount = 40;

    public static readonly JsonStringInt128Converter Default = new();

    protected override int GetMaxCharCountToWrite(Int128 value) => MaxCharCount;
}
