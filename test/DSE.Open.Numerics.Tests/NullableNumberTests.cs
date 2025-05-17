// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NullableNumberTests
{
    [Fact]
    public void InitializeWithValue()
    {
        NullableNumber<int> value = 1;
        Assert.True(value.HasValue);
    }

    [Fact]
    public void InitializeWithNull()
    {
        NullableNumber<int> value = null;
        Assert.False(value.HasValue);
    }

    [Fact]
    public void Null_ToString_NA()
    {
        NullableNumber<int> value = null;
        Assert.Equal(NullableNumber.NoValueLabel, value.ToString());
    }

    [Fact]
    public void EqualValues_AreEqual()
    {
        NullableNumber<int> v2 = 1;
        NullableNumber<int> v1 = 1;
        Assert.Equal(v1, v2);
        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void NullAndValue_NotEqual()
    {
        NullableNumber<int> v2 = 1;
        NullableNumber<int> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void NullAndNull_NotEqual()
    {
        NullableNumber<int> v2 = null;
        NullableNumber<int> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }
}
