// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NaFloatTests
{
    [Fact]
    public void NaFloat_Equality_Tests()
    {
        var na1 = NaFloat<float>.Na;
        var na2 = NaFloat<float>.Na;
        var value1 = new NaFloat<float>(1.23f);
        var value2 = new NaFloat<float>(1.23f);

        Assert.False(na1 == na2); // NaN == NaN is false
        Assert.True(na1.Equals(na2)); // Equals should return true
        Assert.True(na1 != na2); // NaN != NaN is true
        Assert.True(value1 == value2); // Same values are equal
    }

    [Fact]
    public void NaFloat_Arithmetic_Tests()
    {
        var value1 = new NaFloat<float>(2.0f);
        var value2 = new NaFloat<float>(3.0f);
        var na = NaFloat<float>.Na;

        Assert.Equal(new NaFloat<float>(5.0f), value1 + value2);
        Assert.Equal(na, value1 + na); // Any operation with NaN results in NaN
    }

    [Fact]
    public void NaFloat_MinMaxValue_Tests()
    {
        Assert.Equal(float.MinValue, (float)NaFloat<float>.MinValue);
        Assert.Equal(float.MaxValue, (float)NaFloat<float>.MaxValue);
    }

    [Fact]
    public void NaFloat_TryParse_Tests()
    {
        Assert.True(NaFloat<float>.TryParse("1.23", NumberStyles.Float, CultureInfo.InvariantCulture, out var result));
        Assert.Equal(new NaFloat<float>(1.23f), result);

        Assert.False(NaFloat<float>.TryParse("abc", NumberStyles.Float, CultureInfo.InvariantCulture, out _));
        Assert.False(NaFloat<float>.TryParse(null, NumberStyles.Float, CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void NaFloat_GetHashCode_Tests()
    {
        var na = NaFloat<float>.Na;
        var value = new NaFloat<float>(1.23f);

        Assert.Equal(0, na.GetHashCode()); // NaN hash code is 0
        Assert.Equal(1.23f.GetHashCode(), value.GetHashCode());
    }

    [Fact]
    public void NaFloat_ImplicitConversion_Tests()
    {
        NaFloat<float> value = 1.23f;
        Assert.Equal(new NaFloat<float>(1.23f), value);

        NaFloat<float> na = (float?)null;
        Assert.Equal(NaFloat<float>.Na, na);
    }
}
