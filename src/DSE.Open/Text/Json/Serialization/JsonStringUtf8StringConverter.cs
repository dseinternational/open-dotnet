// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringUtf8StringConverter : SpanParsableCharWritingJsonConverter<Utf8String>
{
    public static readonly JsonStringUtf8StringConverter Default = new();

    protected override int GetMaxCharCountToWrite(Utf8String value) => value.Length;
}
