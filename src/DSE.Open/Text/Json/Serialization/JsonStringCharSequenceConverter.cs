// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringCharSequenceConverter : SpanParsableCharWritingJsonConverter<CharSequence>
{
    public static readonly JsonStringCharSequenceConverter Default = new();

    protected override int GetMaxCharCountToWrite(CharSequence value)
    {
        return value.Length;
    }
}
