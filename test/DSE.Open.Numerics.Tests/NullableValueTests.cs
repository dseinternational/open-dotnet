// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NullableValueTests
{
    [Fact]
    public void InitializeWithValue()
    {
        NaValue<string> value = "A";
        Assert.True(value.HasValue);
    }

    [Fact]
    public void InitializeWithNull()
    {
        NaValue<string> value = null;
        Assert.False(value.HasValue);
    }

    [Fact]
    public void Null_ToString_NA()
    {
        NaValue<string> value = null;
        Assert.Equal(NullableValue.NoValueLabel, value.ToString());
    }

    [Fact]
    public void EqualValues_AreEqual()
    {
        NaValue<string> v2 = "A";
        NaValue<string> v1 = "A";
        Assert.Equal(v1, v2);
        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void NullAndValue_NotEqual()
    {
        NaValue<string> v2 = "A";
        NaValue<string> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void NullAndNull_NotEqual()
    {
        NaValue<string> v2 = null;
        NaValue<string> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }
}
