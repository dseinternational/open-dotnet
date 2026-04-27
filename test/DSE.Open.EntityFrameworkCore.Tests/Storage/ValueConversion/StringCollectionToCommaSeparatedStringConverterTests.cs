// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class StringCollectionToCommaSeparatedStringConverterTests
{
    private static readonly string[] s_threeValues = ["alpha", "beta", "gamma"];

    [Fact]
    public void ConvertToJoinsWithCommas()
    {
        var converter = StringCollectionToCommaSeparatedStringConverter.Default;
        var result = converter.ConvertToProvider(s_threeValues)?.ToString();
        Assert.Equal("alpha,beta,gamma", result);
    }

    [Fact]
    public void ConvertFromSplitsAndTrims()
    {
        var converter = StringCollectionToCommaSeparatedStringConverter.Default;
        var result = (ICollection<string>)(converter.ConvertFromProvider("alpha, beta ,gamma")
            ?? throw new InvalidOperationException());
        Assert.Equal(s_threeValues, result);
    }

    [Fact]
    public void ConvertFromEmptyReturnsEmpty()
    {
        var converter = StringCollectionToCommaSeparatedStringConverter.Default;
        var result = (ICollection<string>)(converter.ConvertFromProvider("")
            ?? throw new InvalidOperationException());
        Assert.Empty(result);
    }

    [Fact]
    public void ConvertToEmptyReturnsEmptyString()
    {
        var converter = StringCollectionToCommaSeparatedStringConverter.Default;
        var result = converter.ConvertToProvider(Array.Empty<string>())?.ToString();
        Assert.Equal(string.Empty, result);
    }
}
