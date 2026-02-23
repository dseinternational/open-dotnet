// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class SpeechClarityTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var json = JsonSerializer.Serialize(SpeechClarity.Developing);
        Assert.Equal("50", json);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(90)]
    public void GetOrdinal_ReturnsValue(byte value)
    {
        var speechClarity = (SpeechClarity)value;

        var result = speechClarity.GetOrdinal();

        Assert.Equal(value, result);
    }
}
