// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Tests.Text.Json;

public class JsonStringUInt128ConverterTests
{
    private static readonly Lazy<JsonSerializerOptions> s_options = new(() =>
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringUInt128Converter());
        return options;
    });

    [Theory]
    [InlineData(@"""0""", 0u)]
    [InlineData(@"""1""", 1u)]
    public void Deserialize(string data, ulong expected)
    {
        var result = JsonSerializer.Deserialize<UInt128>(data, s_options.Value);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SerializeDeserializeMaxValue()
    {
        var json = JsonSerializer.Serialize(UInt128.MaxValue, s_options.Value);
        var value = JsonSerializer.Deserialize<UInt128>(json, s_options.Value);
        Assert.Equal(UInt128.MaxValue, value);
    }
}
