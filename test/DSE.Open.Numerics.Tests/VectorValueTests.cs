// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class VectorValueTests : LoggedTestsBase
{
    public VectorValueTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_Float64()
    {
        VectorValue val = -1.358;
        Assert.Equal(-1.358, val.ToFloat64(), 3);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Boolean_roundtrips(bool value)
    {
        VectorValue vectorValue = value;

        Assert.Equal(VectorDataType.Bool, vectorValue.DataType);
        Assert.Equal(value, vectorValue.ToBoolean());
        Assert.Equal(value.ToString(), vectorValue.ToString());
    }

    [Fact]
    public void DateTime64_roundtrips()
    {
        var value = new DateTime64(123456789);
        VectorValue vectorValue = value;

        Assert.Equal(VectorDataType.DateTime64, vectorValue.DataType);
        Assert.Equal(value, vectorValue.ToDateTime64());
    }

    [Fact]
    public void FromValue_WithVectorValue_ShouldReturnOriginalValue()
    {
        VectorValue original = 42;

        var result = VectorValue.FromValue(original);

        Assert.Equal(original, result);
    }

    [Fact]
    public void FromValue_WithUnsupportedValue_ShouldThrowNotSupportedException()
    {
        _ = Assert.Throws<NotSupportedException>(() => VectorValue.FromValue(Guid.NewGuid()));
    }

    [Fact]
    public void ToInt32_WithMismatchedDataType_ShouldThrowInvalidOperationException()
    {
        VectorValue value = 42.0;

        _ = Assert.Throws<InvalidOperationException>(() => value.ToInt32());
    }

    [Fact]
    public void Equals_WithDouble_ShouldOnlyMatchFloat64Values()
    {
        VectorValue float64Value = 42.5;
        VectorValue int32Value = 42;

        Assert.True(float64Value.Equals(42.5));
        Assert.False(float64Value.Equals(42.0));
        Assert.False(int32Value.Equals(42.0));
    }
}
