// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Abstractions.Tests;

public class ConditionDiagnosisCodeTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(ConditionDiagnosisCode value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<ConditionDiagnosisCode>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<ConditionDiagnosisCode> Values { get; } = new TheoryData<ConditionDiagnosisCode>()
    {
        ConditionDiagnosisCode.DownSyndrome,
        ConditionDiagnosisCode.AutismSpectrumDisorder,
        ConditionDiagnosisCode.FragileX,
        ConditionDiagnosisCode.WilliamsSyndrome
    };

}
