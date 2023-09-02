// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations.Tests;

public class YesNoTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(YesNo value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<YesNo>(json);
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public void MustBeInitialized()
    {
        YesNo value = default;
        Assert.Throws<UninitializedValueException<YesNo, AsciiString>>(() => value.ToString());
    }

    public static TheoryData<YesNo> Values { get; } = new TheoryData<YesNo>() { YesNo.No, YesNo.Yes, };
}
