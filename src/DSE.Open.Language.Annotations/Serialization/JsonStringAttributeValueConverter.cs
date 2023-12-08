// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

public class JsonStringAttributeValueConverter : SpanParsableCharWritingJsonConverter<AttributeValue>
{
    public static readonly JsonStringAttributeValueConverter Default = new();

    protected override int GetMaxCharCountToWrite(AttributeValue value)
    {
        return AttributeValue.MaxSerializedCharLength;
    }
}
