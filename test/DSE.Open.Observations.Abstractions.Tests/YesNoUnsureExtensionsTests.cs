// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class YesNoUnsureExtensionsTests
{
    [Theory]
    [InlineData("yes", true, false, false, false)]
    [InlineData("no", false, true, false, true)]
    [InlineData("unsure", false, false, true, true)]
    public void Predicates_ReturnExpected(string raw, bool isYes, bool isNo, bool isUnsure, bool isNoOrUnsure)
    {
        var value = YesNoUnsure.ParseInvariant(raw);

        Assert.Equal(isYes, value.IsYes());
        Assert.Equal(isNo, value.IsNo());
        Assert.Equal(isUnsure, value.IsUnsure());
        Assert.Equal(isNoOrUnsure, value.IsNoOrUnsure());
    }

    [Fact]
    public void IsNoOrUnsure_False_ForYes()
    {
        Assert.False(YesNoUnsure.Yes.IsNoOrUnsure());
    }
}
