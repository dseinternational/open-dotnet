// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Generators.Tests.Functional;

public sealed class ResourceProviderGeneratorTests
{
    [Fact]
    public void GeneratesExpectedTestMethod()
    {
        // Arrange
        const string expected = "Test string";
        const string expectedUs = "Test string EN_US";

        // Act
        var provider = ResourceProvider.Default;
        var str = provider.Test();
        var strUs = provider.Test(new CultureInfo("en-US"));

        // Assert
        Assert.Equal(expected, str);
        Assert.Equal(expectedUs, strUs);
    }

    [Fact]
    public void GeneratesExpectedTestMethodTwo()
    {
        // Arrange
        const string expected = "Test two string";

        // Act
        var provider = ResourceProvider.Default;
        var str = provider.TestTwo();
        var strUs = provider.TestTwo(new CultureInfo("en-US"));

        // Assert
        Assert.Equal(expected, str);
        Assert.Equal(expected, strUs);
    }

    [Fact]
    public void FormattedStringWithTwoHoles()
    {
        // Arrange
        const string name1 = "John";
        const string name2 = "Jane";

        const string expected = $"A test string for {name1} and {name2}";

        // Act
        var provider = ResourceProvider.Default;
        var str = provider.FormattedStringWithTwoHoles(name1, name2);
        var strUs = provider.FormattedStringWithTwoHoles(name1, name2, new CultureInfo("en-US"));

        // Assert
        Assert.Equal(expected, str);
        Assert.Equal(expected, strUs);
    }

    [Fact]
    public void FormattedStringWithTwoHolesAndTrailingText()
    {
        // Arrange
        const string name1 = "John";
        const string name2 = "Jane";

        const string expected = $"A test string for {name1} and {name2} with trailing text";

        // Act
        var provider = ResourceProvider.Default;
        var str = provider.FormattedStringWithTwoHolesAndTrailingText(name1, name2);
        var strUs = provider.FormattedStringWithTwoHolesAndTrailingText(name1, name2, new CultureInfo("en-US"));

        // Assert
        Assert.Equal(expected, str);
        Assert.Equal(expected, strUs);
    }
}
