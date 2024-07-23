// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open;
using DSE.Open.Values;

namespace DSE.Open.Observations;

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
        _ = Assert.Throws<UninitializedValueException<YesNo, AsciiString>>(value.ToString);
    }

    [Fact]
    public void ToBoolean()
    {
        Assert.True(YesNo.Yes.ToBoolean());
        Assert.False(YesNo.No.ToBoolean());
    }

    [Fact]
    public void FromBoolean()
    {
        Assert.Equal(YesNo.Yes, YesNo.FromBoolean(true));
        Assert.Equal(YesNo.No, YesNo.FromBoolean(false));
    }

    public static TheoryData<YesNo> Values { get; } = new() { YesNo.No, YesNo.Yes, };
}
