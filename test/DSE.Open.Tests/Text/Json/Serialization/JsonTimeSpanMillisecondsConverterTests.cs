// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

public class JsonTimeSpanMillisecondsConverterTests
{
    [Fact]
    public void Serialize()
    {
        var converter = new JsonTimeSpanMillisecondsConverter();
        var buffer = new ArrayBufferWriter<byte>(32);
        using var writer = new Utf8JsonWriter(buffer);
        converter.Write(writer, TimeSpan.FromMilliseconds(968485.84698), JsonSerializerOptions.Default);
        writer.Flush();
        Assert.Equal(968485, JsonSerializer.Deserialize<long>(buffer.WrittenSpan, JsonSerializerOptions.Default));
    }

    [Fact]
    public void Deserialize()
    {
        var converter = new JsonTimeSpanMillisecondsConverter();
        var json = "968485"u8;
        var reader = new Utf8JsonReader(json);
        _ = reader.Read();
        var result = converter.Read(ref reader, typeof(TimeSpan), JsonSerializerOptions.Default);
        Assert.Equal(TimeSpan.FromMilliseconds(968485), result);
    }
}
