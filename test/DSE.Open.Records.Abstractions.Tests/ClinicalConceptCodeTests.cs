// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Abstractions.Tests;

public class ClinicalConceptCodeTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(ClinicalConceptCode value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<ClinicalConceptCode>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<ClinicalConceptCode> Values { get; } = new TheoryData<ClinicalConceptCode>()
    {
        (ClinicalConceptCode)41040004,
        (ClinicalConceptCode)35919005,
        (ClinicalConceptCode)229746007,
    };

}
