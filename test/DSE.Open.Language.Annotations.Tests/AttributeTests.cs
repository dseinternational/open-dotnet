// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class AttributeTests
{
    [Theory]
    [InlineData("CorrectForm=are", "CorrectForm", "are")]
    [InlineData("SpaceAfter=No", "SpaceAfter", "No")]
    [InlineData("Entity=(1-place-Tulsa)", "Entity", "(1-place-Tulsa)")]
    public void ParseNameAndValue(string feature, string name, string value)
    {
        var f = Attribute.ParseInvariant(feature);
        Assert.Equal(name, f.Name.ToStringInvariant());
        Assert.Equal(value, f.Value.ToStringInvariant());
    }

    [Theory]
    [InlineData("Case=Acc,Dat", "Case", "Acc", "Dat")]
    public void ParseNameAndValues(string feature, string name, string value0, string value1)
    {
        var f = Attribute.ParseInvariant(feature);
        Assert.Equal(name, f.Name.ToStringInvariant());
        Assert.Equal(value0, f.Value.ToStringInvariant());
        Assert.Equal(value0, f.Values[0].ToStringInvariant());
        Assert.Equal(value1, f.Values[1].ToStringInvariant());
    }

    [Theory]
    [InlineData("LGloss=(něco_stojí_peníze)")]
    [InlineData("Lang=en")]
    [InlineData("Entity=(1-place-Tulsa)")]
    [InlineData("LNumValue=1000000000")]
    public void ParseAndFormat(string feature)
    {
        var f = Attribute.ParseInvariant(feature);
        var s = f.ToStringInvariant();
        Assert.Equal(feature, s);
    }

}
