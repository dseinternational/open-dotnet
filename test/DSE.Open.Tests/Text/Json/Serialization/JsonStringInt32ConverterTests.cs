// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Tests.Text.Json.Serialization;

public class JsonStringInt32ConverterTests
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        Converters =
        {
            new JsonStringInt32Converter()
        }
    };

    [Theory]
    [InlineData(@"""0""", 0)]
    [InlineData(@"""1""", 1)]
    [InlineData(@"""2147483647""", int.MaxValue)]
    [InlineData(@"""-2147483648""", int.MinValue)]
    public void Deserialize(string data, int expected)
    {
        var result = JsonSerializer.Deserialize<int>(data, s_options);
        Assert.Equal(expected, result);
    }
}
