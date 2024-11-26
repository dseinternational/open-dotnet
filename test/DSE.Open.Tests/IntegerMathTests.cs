// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class IntegerMathTests
{
    [Fact]
    public void Int32_Divide()
    {
        Assert.Equal(4, 11.DivideByRoundUp(3));
    }
}
