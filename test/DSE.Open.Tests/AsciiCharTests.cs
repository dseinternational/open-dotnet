// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiCharTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharBytes), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(byte value)
    {
        var v = (AsciiChar)value;
        Assert.Equal(value, (byte)v);
    }
}
