// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonValueObjectConverterTests
{
    private static readonly JsonSerializerOptions s_options = CreateOptions();

    [Fact]
    public void Deserialize_Null_ShouldReturnNull()
    {
        var value = JsonSerializer.Deserialize<object>("null", s_options);

        Assert.Null(value);
    }

    [Fact]
    public void Deserialize_Object_ShouldReturnJsonElement()
    {
        var value = JsonSerializer.Deserialize<object>("""{"name":"test","count":3}""", s_options);

        var element = Assert.IsType<JsonElement>(value);
        Assert.Equal(JsonValueKind.Object, element.ValueKind);
        Assert.Equal("test", element.GetProperty("name").GetString());
        Assert.Equal(3, element.GetProperty("count").GetInt32());
    }

    [Fact]
    public void Deserialize_Array_ShouldReturnJsonElement()
    {
        var value = JsonSerializer.Deserialize<object>("[1,2,3]", s_options);

        var element = Assert.IsType<JsonElement>(value);
        Assert.Equal(JsonValueKind.Array, element.ValueKind);
        Assert.Equal(3, element.GetArrayLength());
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(JsonValueObjectConverter.Default);
        return options;
    }
}
