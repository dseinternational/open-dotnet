// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

public class JsonDateTimeOffsetUnixTimeMillisecondsConverterTests
{
    private readonly DateTimeOffset _testTime = new(2025, 7, 23, 8, 35, 59, 686, TimeSpan.Zero);

    [Fact]
    public void Serialize()
    {
        var converter = new JsonDateTimeOffsetUnixTimeMillisecondsConverter();
        var buffer = new ArrayBufferWriter<byte>(32);
        using var writer = new Utf8JsonWriter(buffer);
        converter.Write(writer, _testTime, JsonSerializerOptions.Default);
        writer.Flush();
        Assert.Equal("1753259759686"u8, buffer.WrittenSpan);
    }

    [Fact]
    public void Deserialize()
    {
        var converter = new JsonDateTimeOffsetUnixTimeMillisecondsConverter();
        var json = "1753259759686"u8;
        var reader = new Utf8JsonReader(json);
        _ = reader.Read();
        var result = converter.Read(ref reader, typeof(TimeSpan), JsonSerializerOptions.Default);
        Assert.Equal(_testTime, result);
    }
}
