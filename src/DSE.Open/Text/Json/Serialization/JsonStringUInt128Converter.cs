// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringUInt128Converter : SpanParsableCharWritingJsonConverter<UInt128>
{
    private const int MaxCharCount = 39;

    public static readonly JsonStringUInt128Converter Default = new();

    protected override int GetMaxCharCountToWrite(UInt128 value) => MaxCharCount;
}
