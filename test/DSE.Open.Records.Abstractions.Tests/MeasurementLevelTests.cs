// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Tests;

public class MeasurementLevelTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(MeasurementLevel value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<MeasurementLevel>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<MeasurementLevel> Values { get; } = new()
    {
        MeasurementLevel.Binary,
        MeasurementLevel.ExtensiveRatio,
        MeasurementLevel.Absolute,
    };
}
