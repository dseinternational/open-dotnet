﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class NumericVectorTestsSubtract
{

    [Fact]
    public void SubtractInPlace_Int32_Zeroes_Ones()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = Vector.CreateZeroes<int>(6);
        VectorPrimitives.SubtractInPlace(v1, v2);
        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        Assert.Equal(v1, v3);
    }
}
