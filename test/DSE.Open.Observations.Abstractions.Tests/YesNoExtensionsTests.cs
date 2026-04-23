// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class YesNoExtensionsTests
{
    [Fact]
    public void IsYes_True_ForYes()
    {
        Assert.True(YesNo.Yes.IsYes());
    }

    [Fact]
    public void IsYes_False_ForNo()
    {
        Assert.False(YesNo.No.IsYes());
    }

    [Fact]
    public void IsNo_True_ForNo()
    {
        Assert.True(YesNo.No.IsNo());
    }

    [Fact]
    public void IsNo_False_ForYes()
    {
        Assert.False(YesNo.Yes.IsNo());
    }
}
