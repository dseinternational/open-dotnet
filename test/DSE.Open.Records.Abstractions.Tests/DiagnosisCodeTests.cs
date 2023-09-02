// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Abstractions.Tests;

public class DiagnosisCodeTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(DiagnosisCode value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<DiagnosisCode>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<DiagnosisCode> Values { get; } = new TheoryData<DiagnosisCode>()
    {
        DiagnosisCode.DownSyndrome,
        DiagnosisCode.AutismSpectrumDisorder,
        DiagnosisCode.FragileX,
        DiagnosisCode.WilliamsSyndrome
    };

}
