// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class TokenIndexTests
{
    [Theory]
    [InlineData("1", 1, 1, null)]
    [InlineData("2", 2, 2, null)]
    [InlineData("20", 20, 20, null)]
    [InlineData("2-3", 2, 3, null)]
    [InlineData("1.2", 1, 1, 2)]
    public void CanParseIds(string value, int start, int rangeEnd, int? subId)
    {
        var tokenId = TokenIndex.ParseInvariant(value);
        Assert.Equal(start, tokenId.Start);
        Assert.Equal(subId, tokenId.EmptyId);
        Assert.Equal(rangeEnd, tokenId.End);
    }

    [Theory]
    [InlineData("1", 1, null, null)]
    [InlineData("2", 2, 2, null)]
    [InlineData("20", 20, null, null)]
    [InlineData("2-3", 2, 3, null)]
    [InlineData("1.2", 1, null, 2)]
    public void IdsToString(string value, int start, int? rangeEnd, int? subId)
    {
        var tokenId = new TokenIndex(start, rangeEnd, subId);

        var str = tokenId.ToString();

        Assert.Equal(value, str);
    }
}
