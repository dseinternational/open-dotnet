// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class BehaviorFrequencyTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = BehaviorFrequency.FromValue(50);
        var json = JsonSerializer.Serialize(value);
        Assert.Equal("50", json);
    }
}
