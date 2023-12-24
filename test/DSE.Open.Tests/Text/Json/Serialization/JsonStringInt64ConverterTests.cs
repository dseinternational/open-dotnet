// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Tests.Text.Json;

public class JsonStringInt64ConverterTests
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        Converters =
        {
            new JsonStringInt64Converter()
        }
    };

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
        var result = JsonSerializer.Deserialize<long>(data, s_options);
        Assert.Equal(expected, result);
    }
}
