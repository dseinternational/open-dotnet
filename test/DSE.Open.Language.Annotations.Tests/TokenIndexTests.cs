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

    [Fact]
    public void TryParse_WithString()
    {
        // Arrange
        var expected = new TokenIndex(1, 2);
        var indexStr = expected.ToString();

        // Act
        var result = TokenIndex.TryParse(indexStr, null, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void New_WithEndBeforeStart_ShouldThrowArgumentOutOfRangeException()
    {
        // Act
        static void Act() => _ = new TokenIndex(2, 1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Theory]
    [MemberData(nameof(EqualsData))]
    public void Equals_ShouldReturnTrue(TokenIndex original, TokenIndex next)
    {
        Assert.Equal(original, next);
    }

    [Theory]
    [MemberData(nameof(NotEqualData))]
    public void NotEqual_ShouldReturnFalse(TokenIndex original, TokenIndex next)
    {
        Assert.NotEqual(original, next);
    }

    [Theory]
    [MemberData(nameof(GreaterThanData))]
    public void GreaterThan_ShouldReturnTrue(TokenIndex left, TokenIndex right)
    {
        Assert.True(left > right);
    }

    [Theory]
    [MemberData(nameof(LessThanData))]
    public void LessThan_ShouldReturnTrue(TokenIndex left, TokenIndex right)
    {
        Assert.True(left < right);
    }

    public static IEnumerable<TheoryDataRow<TokenIndex, TokenIndex>> EqualsData()
    {
        yield return (new TokenIndex(1), new TokenIndex(1));
        yield return (new TokenIndex(1, 4), new TokenIndex(1, 4));
        yield return (new TokenIndex(1, emptyId: 4), new TokenIndex(1, emptyId: 4));
    }

    public static IEnumerable<TheoryDataRow<TokenIndex, TokenIndex>> NotEqualData()
    {
        yield return (new TokenIndex(1), new TokenIndex(2));
        yield return (new TokenIndex(1), new TokenIndex(1, 4));
        yield return (new TokenIndex(1, 4), new TokenIndex(1, 5));
        yield return (new TokenIndex(1, 4), new TokenIndex(2, 4));
        yield return (new TokenIndex(1, emptyId: 1), new TokenIndex(1, emptyId: 2));
    }

    public static IEnumerable<TheoryDataRow<TokenIndex, TokenIndex>> LessThanData()
    {
        yield return (new TokenIndex(1), new TokenIndex(2));
        yield return (new TokenIndex(1, 2), new TokenIndex(1, 3));
        yield return (new TokenIndex(1, emptyId: 1), new TokenIndex(1, emptyId: 2));
    }

    public static IEnumerable<TheoryDataRow<TokenIndex, TokenIndex>> GreaterThanData()
    {
        yield return (new TokenIndex(2), new TokenIndex(1));
        yield return (new TokenIndex(1, 3), new TokenIndex(1, 2));
        yield return (new TokenIndex(1, emptyId: 2), new TokenIndex(1, emptyId: 1));
    }
}
