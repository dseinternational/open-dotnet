// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public class MeasurementLevelTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(MeasurementLevel value)
    {
        AssertJson.Roundtrip(value);
    }

    public static TheoryData<MeasurementLevel> Values { get; } =
    [
        MeasurementLevel.Binary,
        MeasurementLevel.ExtensiveRatio,
        MeasurementLevel.Absolute,
    ];
}
