// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;

namespace DSE.Open.Labels.Tests;

public class ResourceLabelDescriptionProviderTests
{
    [Theory]
    [InlineData(MyValues.Value1, "en", "One")]
    [InlineData(MyValues.Value2, "en", "Two")]
    [InlineData(MyValues.Value3, "en", "Three")]
    [InlineData(MyValues.Value1, "en-GB", "One")]
    [InlineData(MyValues.Value2, "en-GB", "Two")]
    [InlineData(MyValues.Value3, "en-GB", "Three")]
    [InlineData(MyValues.Value1, "fr-FR", "Un")]
    [InlineData(MyValues.Value2, "fr-FR", "Deux")]
    [InlineData(MyValues.Value3, "fr-FR", "Trois")]
    [InlineData(MyValues.Value1, "fr", "Un")]
    [InlineData(MyValues.Value2, "fr", "Deux")]
    [InlineData(MyValues.Value3, "fr", "Trois")]
    public void GetLabelReturnsExpectedValues(MyValues value, string culture, string expected)
    {
        Assert.Equal(expected, TestResourceLabelDescriptionProvider.Default.GetLabel(value, new CultureInfo(culture)));
    }
}
