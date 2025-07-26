// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Text.Json.Serialization;

public sealed class ByteSequenceJsonConverterTests
{
    private static readonly JsonWriterOptions s_writerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    [Fact]
    public void CanConvert_ShouldReturnTrueForSupportedTypes()
    {
        var factory = new ByteSequenceJsonConverter();
        Assert.True(factory.CanConvert(typeof(byte[])));
        Assert.True(factory.CanConvert(typeof(ReadOnlyMemory<byte>)));
        Assert.True(factory.CanConvert(typeof(Memory<byte>)));
        Assert.True(factory.CanConvert(typeof(List<byte>)));
        Assert.True(factory.CanConvert(typeof(Collection<byte>)));
    }

    [Fact]
    public void Serialize_Deserialize_Array()
    {
        var original = new byte[] { 1, 2, 3, 4, 5 };
        var deserialized = SerializeDeserialize(original);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Serialize_Deserialize_Memory()
    {
        var original = new Memory<byte>([1, 2, 3, 4, 5]);
        var deserialized = SerializeDeserialize(original);
        Assert.Equal(original.Span, deserialized.Span);
    }

    [Fact]
    public void Serialize_Deserialize_List()
    {
        IEnumerable<byte> original = new List<byte>([1, 2, 3, 4, 5]);
        var deserialized = SerializeDeserialize(original);
        Assert.Equal(original, deserialized);
    }

    private T SerializeDeserialize<T>(T original)
    {
        var factory = new ByteSequenceJsonConverter();
        var converter = (JsonConverter<T>?)factory.CreateConverter(typeof(T), JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(converter);

        using var stream = new MemoryStream();

        using (var writer = new Utf8JsonWriter(stream, s_writerOptions))
        {
            converter.Write(writer, original, JsonSharedOptions.RelaxedJsonEscaping);
        }

        stream.Position = 0;

        var reader = new Utf8JsonReader(stream.ToArray());

        Assert.True(reader.Read());
        Assert.Equal(JsonTokenType.StartArray, reader.TokenType);

        var deserialized = converter.Read(ref reader, typeof(T), JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(deserialized);

        return deserialized;
    }
}
