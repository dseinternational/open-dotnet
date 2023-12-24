// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonStringTagConverter : SpanParsableCharWritingJsonConverter<Tag>
{
    public static readonly JsonStringTagConverter Default = new();

    protected override int GetMaxCharCountToWrite(Tag value)
    {
        return Tag.MaxLength;
    }
}
