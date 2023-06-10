// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Tests.Text.Json;

public class JsonStringInt32ConverterTests
{
    [Theory]
    [InlineData(@"""0""", 0)]
    [InlineData(@"""1""", 1)]
    [InlineData(@"""2147483647""", int.MaxValue)]
    [InlineData(@"""-2147483648""", int.MinValue)]
    public void Deserialize(string data, int expected)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringInt32Converter());
        var result = JsonSerializer.Deserialize<int>(data, options);
        Assert.Equal(expected, result);
    }
}
