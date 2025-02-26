// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class NumericVectorTests
{

    [Fact]
    public void AddInPace_Int32_Zeroes_Ones()
    {
        var v1 = NumericVector.CreateZeroes<int>(6);
        var v2 = NumericVector.CreateOnes<int>(6);
        NumericVector.AddInPace(v1, v2);
        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        Assert.True(v1.SequenceEqual(v2));
    }
}
