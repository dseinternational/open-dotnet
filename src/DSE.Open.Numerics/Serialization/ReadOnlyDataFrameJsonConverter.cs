// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Data;

namespace DSE.Open.Numerics.Serialization;

public class ReadOnlyDataFrameJsonConverter : JsonConverter<IReadOnlyDataFrame>
{
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(IReadOnlyDataFrame));
    }

    public override IReadOnlyDataFrame Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyDataFrame value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
