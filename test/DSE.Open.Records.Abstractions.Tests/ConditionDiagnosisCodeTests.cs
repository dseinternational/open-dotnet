// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Tests;

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

    [Fact]
    public static void HaveCorrectConceptIds()
    {
        Assert.Equal(41040004, ConditionDiagnosisCode.DownSyndrome);
        Assert.Equal(613003, ConditionDiagnosisCode.FragileX);
        Assert.Equal(63247009, ConditionDiagnosisCode.WilliamsSyndrome);
        Assert.Equal(35919005, ConditionDiagnosisCode.AutismSpectrumDisorder);
        Assert.Equal(280032002, ConditionDiagnosisCode.DevelopmentalLanguageDisorder);
        Assert.Equal(229746007, ConditionDiagnosisCode.SpecificLanguageImpairment);
    }

    public static TheoryData<ConditionDiagnosisCode> Values { get; } = new TheoryData<ConditionDiagnosisCode>()
    {
        ConditionDiagnosisCode.DownSyndrome,
        ConditionDiagnosisCode.AutismSpectrumDisorder,
        ConditionDiagnosisCode.FragileX,
        ConditionDiagnosisCode.WilliamsSyndrome
    };

}
