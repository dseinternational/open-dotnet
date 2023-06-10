// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiCharSequenceTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharSequenceStrings), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(string value)
    {
        var v = (AsciiCharSequence)value;
        Assert.Equal(value, v.ToString());
    }
}
