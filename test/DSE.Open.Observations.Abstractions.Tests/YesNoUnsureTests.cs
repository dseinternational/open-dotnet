// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

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

    public static TheoryData<YesNoUnsure> Values { get; } = new() { YesNoUnsure.No, YesNoUnsure.Yes, YesNoUnsure.Unsure, };
}
