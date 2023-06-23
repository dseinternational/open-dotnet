// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringUtf8StringConverter : JsonConverter<Utf8String>
{
    public static readonly JsonStringUtf8StringConverter Default = new();

    public override Utf8String Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            JsonExceptionHelper.ThrowJsonException("Expected string value");
        }

        return new(reader.ValueSpan.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, Utf8String value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);
        writer.WriteStringValue(value.Span);
    }
}
