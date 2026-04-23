// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class TypeHelperTests
{
    [Theory]
    [InlineData(typeof(int?))]
    [InlineData(typeof(double?))]
    [InlineData(typeof(DateTime?))]
    [InlineData(typeof(Guid?))]
    public void IsNullableType_NullableValueType_ReturnsTrue(Type type)
    {
        Assert.True(TypeHelper.IsNullableType(type));
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(string))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(object))]
    [InlineData(typeof(List<int>))]
    public void IsNullableType_NonNullable_ReturnsFalse(Type type)
    {
        Assert.False(TypeHelper.IsNullableType(type));
    }

    [Fact]
    public void IsNullableType_NullType_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => TypeHelper.IsNullableType(null!));
    }

    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(int))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(long))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    public void IsNumericType_Numeric_ReturnsTrue(Type type)
    {
        Assert.True(TypeHelper.IsNumericType(type));
    }

    [Theory]
    [InlineData(typeof(bool))]
    [InlineData(typeof(char))]
    [InlineData(typeof(string))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(object))]
    [InlineData(typeof(int?))]
    public void IsNumericType_NonNumeric_ReturnsFalse(Type type)
    {
        Assert.False(TypeHelper.IsNumericType(type));
    }

    [Fact]
    public void IsNumericType_NullType_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => TypeHelper.IsNumericType(null!));
    }
}
