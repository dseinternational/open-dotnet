// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Tests.Text.Json.Serialization;

public class JsonBinarySerializerTests
{
    [Fact]
    public void SerializeDeserializeString()
    {
        var value = "A string value";
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<string>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeInt32()
    {
        var value = 123456;
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<int>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeInt32Array()
    {
        int[] value = [1, 2, 3, 4, 123456];
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<int[]>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }
}
