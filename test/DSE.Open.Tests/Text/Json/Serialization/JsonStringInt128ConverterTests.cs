// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringInt128ConverterTests
{
    private static readonly Lazy<JsonSerializerOptions> s_options = new(() =>
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringInt128Converter());
        return options;
    });

    [Theory]
    [InlineData("""
                "0"
                """, 0)]
    [InlineData("""
                "1"
                """, 1)]
    [InlineData("""
                "9223372036854775807"
                """, long.MaxValue)]
    [InlineData("""
                "-9223372036854775808"
                """, long.MinValue)]
    public void Deserialize(string data, long expected)
    {
        var result = JsonSerializer.Deserialize<Int128>(data, s_options.Value);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SerializeDeserializeMaxValue()
    {
        var json = JsonSerializer.Serialize(Int128.MaxValue, s_options.Value);
        var value = JsonSerializer.Deserialize<Int128>(json, s_options.Value);
        Assert.Equal(Int128.MaxValue, value);
    }

    [Fact]
    public void SerializeDeserializeMinValue()
    {
        var json = JsonSerializer.Serialize(Int128.MinValue, s_options.Value);
        var value = JsonSerializer.Deserialize<Int128>(json, s_options.Value);
        Assert.Equal(Int128.MinValue, value);
    }
}
