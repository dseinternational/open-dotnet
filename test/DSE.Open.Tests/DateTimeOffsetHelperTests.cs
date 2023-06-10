// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class DateTimeOffsetHelperTests
{
    [Fact]
    public void Roundtrip()
    {
        var now = DateTimeOffset.Now;
        var str = now.ToIso8601String();
        var now2 = DateTimeOffsetHelper.ParseIso8601(str);
        Assert.Equal(now, now2);
    }
}
