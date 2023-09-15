// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Tests;

public class BiologicalSexTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(BiologicalSex value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<BiologicalSex>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<BiologicalSex> Values { get; } = new TheoryData<BiologicalSex>()
    {
        BiologicalSex.Female,
        BiologicalSex.Male,
    };
}
