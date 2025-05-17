// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NullableValueTests
{
    [Fact]
    public void InitializeWithValue()
    {
        NullableValue<string> value = "A";
        Assert.True(value.HasValue);
    }

    [Fact]
    public void InitializeWithNull()
    {
        NullableValue<string> value = null;
        Assert.False(value.HasValue);
    }

    [Fact]
    public void Null_ToString_NA()
    {
        NullableValue<string> value = null;
        Assert.Equal(NullableValue.NoValueLabel, value.ToString());
    }

    [Fact]
    public void EqualValues_AreEqual()
    {
        NullableValue<string> v2 = "A";
        NullableValue<string> v1 = "A";
        Assert.Equal(v1, v2);
        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void NullAndValue_NotEqual()
    {
        NullableValue<string> v2 = "A";
        NullableValue<string> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void NullAndNull_NotEqual()
    {
        NullableValue<string> v2 = null;
        NullableValue<string> v1 = null;
        Assert.NotEqual(v1, v2);
        Assert.False(v1.Equals(v2));
    }
}
