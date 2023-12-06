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

        var valueLength = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        var unencodedUtf = new byte[valueLength];

        var bytesWritten = reader.CopyString(unencodedUtf);

        return new(new ReadOnlyMemory<byte>(unencodedUtf, 0, bytesWritten));
    }

    public override void Write(Utf8JsonWriter writer, Utf8String value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.Span);
    }
}
