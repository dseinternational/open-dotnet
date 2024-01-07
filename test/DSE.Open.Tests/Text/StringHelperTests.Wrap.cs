// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using DSE.Open.Text;

namespace DSE.Open.Tests.Text;

public partial class StringHelperTests
{
    [Fact]
    public void Wrap_Chars()
    {
        var wrapped = StringHelper.Wrap('[', ']', 'A');
        Assert.Equal("[A]", wrapped);
    }

    [Fact]
    public void Wrap_Chars_same()
    {
        var wrapped = StringHelper.Wrap('|', 'A');
        Assert.Equal("|A|", wrapped);
    }

    [Fact]
    public void Wrap_Strings()
    {
        var wrapped = StringHelper.Wrap("{{", "}}", "hello");
        Assert.Equal("{{hello}}", wrapped);
    }

    [Fact]
    public void Wrap_Strings_same()
    {
        var wrapped = StringHelper.Wrap("||", "hello");
        Assert.Equal("||hello||", wrapped);
    }

    [Fact]
    public void Wrap_Int32()
    {
        var wrapped = StringHelper.Wrap('[', ']', 100);
        Assert.Equal("[100]", wrapped);
    }

    [Fact]
    public void Wrap_Int64()
    {
        var wrapped = StringHelper.Wrap('[', ']', 9223372036854775807);
        Assert.Equal("[9223372036854775807]", wrapped);
    }

    [Fact]
    public void Wrap_Double_char_formatted()
    {
        var wrapped = StringHelper.Wrap('[', ']', 92233.75807, "0.00", CultureInfo.InvariantCulture);
        Assert.Equal("[92233.76]", wrapped);
    }

    [Fact]
    public void Wrap_Double_string_formatted()
    {
        var wrapped = StringHelper.Wrap("{{", "}}", 92233.75807, "0.00", CultureInfo.InvariantCulture);
        Assert.Equal("{{92233.76}}", wrapped);
    }

    [Fact]
    public void WrapRange_Strings_array()
    {
        string[] values = ["one", "two", "three"];
        var wrapped = StringHelper.WrapRange("{{", "}}", values).ToArray();
        Assert.Equal("{{one}}", wrapped[0]);
        Assert.Equal("{{two}}", wrapped[1]);
        Assert.Equal("{{three}}", wrapped[2]);
    }

    [Fact]
    public void WrapRange_Strings_list()
    {
        List<string> values = ["one", "two", "three"];
        var wrapped = StringHelper.WrapRange("{{", "}}", values).ToArray();
        Assert.Equal("{{one}}", wrapped[0]);
        Assert.Equal("{{two}}", wrapped[1]);
        Assert.Equal("{{three}}", wrapped[2]);
    }

    [Fact]
    public void WrapRange_Strings_Collection()
    {
        Collection<string> values = ["one", "two", "three"];
        var wrapped = StringHelper.WrapRange("{{", "}}", values).ToArray();
        Assert.Equal("{{one}}", wrapped[0]);
        Assert.Equal("{{two}}", wrapped[1]);
        Assert.Equal("{{three}}", wrapped[2]);
    }

    [Fact]
    public void WrapRange_doubles_array_formatteed()
    {
        double[] values = [1.5687, 206.9715, 3.0, 42, 1111.111111111];
        var wrapped = StringHelper.WrapRange("{{", "}}", values, "0.00", CultureInfo.InvariantCulture).ToArray();
        Assert.Equal("{{1.57}}", wrapped[0]);
        Assert.Equal("{{206.97}}", wrapped[1]);
        Assert.Equal("{{3.00}}", wrapped[2]);
        Assert.Equal("{{42.00}}", wrapped[3]);
        Assert.Equal("{{1111.11}}", wrapped[4]);
    }

    [Fact]
    public void WrapRange_doubles_collection_formatteed()
    {
        Collection<double> values = [1.5687, 206.9715, 3.0, 42, 1111.111111111];
        var wrapped = StringHelper.WrapRange("{{", "}}", values, "0.00", CultureInfo.InvariantCulture).ToArray();
        Assert.Equal("{{1.57}}", wrapped[0]);
        Assert.Equal("{{206.97}}", wrapped[1]);
        Assert.Equal("{{3.00}}", wrapped[2]);
        Assert.Equal("{{42.00}}", wrapped[3]);
        Assert.Equal("{{1111.11}}", wrapped[4]);
    }
}
