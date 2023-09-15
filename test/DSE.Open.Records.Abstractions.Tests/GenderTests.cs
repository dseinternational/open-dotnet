// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Tests;

public class GenderTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(Gender value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<Gender>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<Gender> Values { get; } = new TheoryData<Gender>()
    {
        Gender.Female,
        Gender.Male,
        Gender.Other,
    };
}
