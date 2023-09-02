// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations.Tests;

public class YesNoUnsureTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(YesNoUnsure value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<YesNoUnsure>(json);
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public void MustBeInitialized()
    {
        YesNoUnsure value = default;
        Assert.Throws<UninitializedValueException<YesNoUnsure, AsciiString>>(() => value.ToString());
    }

    public static TheoryData<YesNoUnsure> Values { get; } = new TheoryData<YesNoUnsure>() { YesNoUnsure.No, YesNoUnsure.Yes, YesNoUnsure.Unsure, };
}
