// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class CharSequenceExtensionTests
{
    [Fact]
    public void Decimal_ToCharSequence_formats_correctly()
    {
        var cs = decimal.MaxValue.ToCharSequence(default, CultureInfo.InvariantCulture);
        Assert.Equal(new CharSequence("79228162514264337593543950335"), cs);
    }

    [Fact]
    public void Double_ToCharSequence_formats_correctly()
    {
        var cs = double.MaxValue.ToCharSequence(default, CultureInfo.InvariantCulture);
        Assert.Equal(new CharSequence("1.7976931348623157E+308"), cs);
    }

    [Fact]
    public void String_ToCharSequence_formats_correctly()
    {
        var cs = "A string".ToCharSequence();
        Assert.Equal(new CharSequence("A string"), cs);
    }

    [Fact]
    public void Long_string_ToCharSequence_formats_correctly()
    {
        var longString = string.Join(" ", Enumerable.Range(0, 300).Select(i => "A string."));
        var cs = longString.ToCharSequence();
        Assert.Equal(new CharSequence(longString), cs);
    }
}
