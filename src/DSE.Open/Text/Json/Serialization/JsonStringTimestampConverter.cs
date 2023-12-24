// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringTimestampConverter : SpanParsableCharWritingJsonConverter<Timestamp>
{
    public static readonly JsonStringTimestampConverter Default = new();

    protected override int GetMaxCharCountToWrite(Timestamp value)
    {
        return Timestamp.Base64Length;
    }
}
