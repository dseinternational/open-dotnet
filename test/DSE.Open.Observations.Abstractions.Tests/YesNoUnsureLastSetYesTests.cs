// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations.Abstractions.Tests;

public class YesNoUnsureLastSetYesTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(YesNoUnsureLastSetYes value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<YesNoUnsureLastSetYes>(json);
        Assert.Equal(value, deserialized);
    }

    public static TheoryData<YesNoUnsureLastSetYes> Values { get; } = new TheoryData<YesNoUnsureLastSetYes>()
    {
        (YesNoUnsureLastSetYes)YesNoUnsure.No,
        (YesNoUnsureLastSetYes)YesNoUnsure.Yes,
        (YesNoUnsureLastSetYes)YesNoUnsure.Unsure,
        new(YesNoUnsure.No, DateTimeOffset.Now.AddYears(-1)),
        new(YesNoUnsure.Yes, DateTimeOffset.Now.AddMonths(-1)),
        new(YesNoUnsure.Unsure, DateTimeOffset.Now.AddDays(-1)),
    };
}
