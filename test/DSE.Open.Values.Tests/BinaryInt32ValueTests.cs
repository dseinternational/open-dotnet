// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.TestValues;

namespace DSE.Open.Values;

public class BinaryInt32ValueTests
{
    [Fact]
    public void Values_with_equal_underlying_values_are_equal()
    {
        var v1 = BinaryInt32Value.False;
        var v2 = BinaryInt32Value.False;
        Assert.Equal(v1, v2);

        var v3 = BinaryInt32Value.True;
        var v4 = BinaryInt32Value.True;
        Assert.Equal(v3, v4);
    }

    [Fact]
    public void Values_with_nonnequal_underlying_values_are_not_equal()
    {
        var v1 = BinaryInt32Value.False;
        var v2 = BinaryInt32Value.True;
        Assert.NotEqual(v1, v2);

        var v3 = BinaryInt32Value.True;
        var v4 = BinaryInt32Value.False;
        Assert.NotEqual(v3, v4);
    }
}
