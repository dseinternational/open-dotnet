// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringAsciiStringConverter : SpanParsableByteWritingJsonConverter<AsciiString>
{
    public static readonly JsonStringAsciiStringConverter Default = new();

    protected override int GetMaxByteCountToWrite(AsciiString value) => value.Length;
}
