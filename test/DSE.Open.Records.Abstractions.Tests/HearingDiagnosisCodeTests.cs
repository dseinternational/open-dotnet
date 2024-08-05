// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Abstractions.Tests;

public class HearingDiagnosisCodeTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(HearingDiagnosisCode value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<HearingDiagnosisCode>(json);
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public static void HaveCorrectConceptIds()
    {
        Assert.Equal(15188001, HearingDiagnosisCode.HearingLoss);
        Assert.Equal(65363002, HearingDiagnosisCode.OtitisMedia);
        Assert.Equal(80327007, HearingDiagnosisCode.OtitisMediaWithEffusion);
        Assert.Equal(77507001, HearingDiagnosisCode.ConductiveAndSensorineuralHearingLoss);
    }

    public static TheoryData<HearingDiagnosisCode> Values { get; } = new()
    {
        HearingDiagnosisCode.HearingLoss,
        HearingDiagnosisCode.SensorineuralHearingLoss,
        HearingDiagnosisCode.ConductiveAndSensorineuralHearingLoss,
        HearingDiagnosisCode.OtitisMediaWithEffusion
    };

}
