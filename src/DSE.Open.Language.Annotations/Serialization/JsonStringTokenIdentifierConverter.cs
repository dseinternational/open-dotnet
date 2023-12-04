// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

public class JsonStringTokenIdentifierConverter : SpanParsableCharWritingJsonConverter<TokenIndex>
{
    public static readonly JsonStringTokenIdentifierConverter Default = new();

    protected override int GetMaxCharCountToWrite(TokenIndex value)
    {
        return TokenIndex.MaxSerializedCharLength;
    }
}
