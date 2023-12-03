// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Abstractions.Tests;

public class WordFeatureTests
{
    [Theory]
    [InlineData("Voice=Pass", "Voice", "Pass")]
    [InlineData("Gender=Masc", "Gender", "Masc")]
    [InlineData("Number=Sing", "Number", "Sing")]
    public void ParseNameAndValue(string feature, string name, string value)
    {
        var f = WordFeature.ParseInvariant(feature);
        Assert.Equal(name, f.Name.ToStringInvariant());
        Assert.Equal(value, f.Value.ToStringInvariant());
    }

    [Theory]
    [InlineData("Case=Acc,Dat", "Case", "Acc", "Dat")]
    public void ParseNameAndValues(string feature, string name, string value0, string value1)
    {
        var f = WordFeature.ParseInvariant(feature);
        Assert.Equal(name, f.Name.ToStringInvariant());
        Assert.Equal(value0, f.Value.ToStringInvariant());
        Assert.Equal(value0, f.Values[0].ToStringInvariant());
        Assert.Equal(value1, f.Values[1].ToStringInvariant());
    }

    [Theory]
    [InlineData("Voice=Pass")]
    [InlineData("Gender=Masc")]
    [InlineData("Number=Sing")]
    public void ParseAndFormat(string feature)
    {
        var f = WordFeature.ParseInvariant(feature);
        var s = f.ToStringInvariant();
        Assert.Equal(feature, s);
    }

}
