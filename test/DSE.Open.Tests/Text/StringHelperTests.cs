// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text;

namespace DSE.Open.Tests.Text;

public class StringHelperTests
{
    [Fact]
    public void Capitalize_Uppercase()
        => Assert.Equal("THIS IS A SENTENCE.", StringHelper.Capitalize("this is a sentence.", CapitalizationStyle.Uppercase));

    [Fact]
    public void ExtractWords_ExtractsWords()
    {
        const string text = "The        cat is\ton the     \t\t\t\t  bed.";
        var result = StringHelper.ExtractWords(text).ToList();
        Assert.Equal("The", result[0]);
        Assert.Equal("cat", result[1]);
        Assert.Equal("is", result[2]);
        Assert.Equal("on", result[3]);
        Assert.Equal("the", result[4]);
        Assert.Equal("bed", result[5]);
    }

    [Fact]
    public void SplitOnWhitespace_Empty()
    {
        var result = StringHelper.SplitOnWhitespace("");
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void SplitOnWhitespace_Null()
    {
        var result = StringHelper.SplitOnWhitespace(null!);
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void SplitOnWhitespace_SpacesAndTabs()
    {
        const string text = "The        cat is\ton the     \t\t\t\t  bed.";
        var result = StringHelper.SplitOnWhitespace(text).ToList();
        Assert.Equal(6, result.Count);
        Assert.Equal("The", result[0]);
        Assert.Equal("cat", result[1]);
        Assert.Equal("is", result[2]);
        Assert.Equal("on", result[3]);
        Assert.Equal("the", result[4]);
        Assert.Equal("bed.", result[5]);
    }

    [Fact]
    public void ExtractDigits_ReturnsOnlyDigits()
        => Assert.Equal("341279065", StringHelper.ExtractDigits("Aoerjugfbn3weu412iuhgf79065@:kcereiounf"));

    [Fact]
    public void ExtractAlphaNumeric_ReturnsOnlyAlphaNumeric()
        => Assert.Equal("Afbn3weu412iuhgf79065zf", StringHelper.ExtractAlphaNumeric("A*f%bn$3we@u412:iuhg;f79,06/5z??f"));

    [Theory]
    [InlineData("", "")]
    [InlineData("LastName", "last_name")]
    [InlineData("lastName", "last_name")]
    [InlineData("ALongTitleForAProduct", "a_long_title_for_a_product")]
    [InlineData("URLValue", "url_value")]
    [InlineData("IPhone", "i_phone")]
    [InlineData("I Phone", "i_phone")]
    [InlineData("I  Phone", "i_phone")]
    [InlineData(" IPhone", "i_phone")]
    [InlineData(" IPhone ", "i_phone")]
    public void SnakeCasingTest(string input, string expected)
    {
        var result = StringHelper.ToSnakeCase(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("I", "i")]
    [InlineData("LastName", "last-name")]
    [InlineData("lastName", "last-name")]
    [InlineData("ALongTitleForAProduct", "a-long-title-for-a-product")]
    public void SlugCasingTest(string input, string expected)
    {
        var result = StringHelper.ToSlugCase(input);
        Assert.Equal(expected, result);
    }

    private const string LargeNoPunctuation = "The cat is on the bed and the dog is on the floor and the mouse is on the table and the horse is in the field and the cow is in the barn";
    private const string LargePunctuation = "The cat is on the bed, and the dog is on the floor, and the mouse is on the table, and the horse is in the field, and the cow is in the barn.";
    private const string SmallNoPunctuation = "The cat is on the bed";
    private const string SmallPunctuation = "The cat is on the bed.";
    private const string SmallLeadingPunctuation = "...The cat is on the bed";
    private const string SmallNoLeadingPunctuation = "The cat is on the bed";
    
    [Theory]
    [InlineData(LargePunctuation, LargeNoPunctuation)]
    [InlineData(SmallPunctuation, SmallNoPunctuation)]
    [InlineData(LargeNoPunctuation, LargeNoPunctuation)]
    [InlineData(SmallNoPunctuation, SmallNoPunctuation)]
    [InlineData(SmallLeadingPunctuation, SmallNoLeadingPunctuation)]
    public void RemovePunctuationTests(string input, string expected)
    {
        // Act
        var actual = StringHelper.RemovePunctuation(input);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}
