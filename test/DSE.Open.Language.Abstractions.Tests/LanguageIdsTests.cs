// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

public sealed class LanguageIdsTests
{
    [Fact]
    public void MinIdValue_is_twelve_digits()
    {
        Assert.Equal(12, LanguageIds.MinIdValue.ToStringInvariant().Length);
        Assert.Equal(100000000001ul, LanguageIds.MinIdValue);
    }

    [Fact]
    public void MaxIdValue_is_twelve_digits()
    {
        Assert.Equal(12, LanguageIds.MaxIdValue.ToStringInvariant().Length);
        Assert.Equal(999999999999ul, LanguageIds.MaxIdValue);
    }

    [Fact]
    public void MaxRange_equals_inclusive_span_between_bounds()
    {
        Assert.Equal(LanguageIds.MaxIdValue - LanguageIds.MinIdValue, LanguageIds.MaxRange);
    }

    [Fact]
    public void Min_is_less_than_max()
    {
        Assert.True(LanguageIds.MinIdValue < LanguageIds.MaxIdValue);
    }

    [Theory]
    [InlineData(WordIdValue)]
    [InlineData(WordMeaningIdValue)]
    [InlineData(SentenceIdValue)]
    public void Identifier_types_accept_boundary_values(ulong value)
    {
        Assert.True(value >= LanguageIds.MinIdValue);
        Assert.True(value <= LanguageIds.MaxIdValue);
    }

    private const ulong WordIdValue = 258089004501;
    private const ulong WordMeaningIdValue = LanguageIds.MinIdValue;
    private const ulong SentenceIdValue = LanguageIds.MaxIdValue;
}
