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
}
