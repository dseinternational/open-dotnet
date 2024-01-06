// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text;

namespace DSE.Open.Tests.Text;

public partial class StringHelperTests
{
    [Fact]
    public void Joins_array_of_string()
    {
        string[] values = ["one", "two", "three"];
        var joined = StringHelper.Join(", ", values, " and ");
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void Joins_list_of_string()
    {
        List<string> values = ["one", "two", "three"];
        var joined = StringHelper.Join(", ", values, " and ");
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void Joins_enumerable_of_string()
    {
        var joined = StringHelper.Join(", ", GetValues(), " and ");
        Assert.Equal("one, two and three", joined);

        static IEnumerable<string> GetValues()
        {
            yield return "one";
            yield return "two";
            yield return "three";
        }
    }

    [Fact]
    public void Joins_array_of_int()
    {
        int[] values = [1, 2, 3, 4, 5];
        var joined = StringHelper.Join(", ", values, " and ");
        Assert.Equal("1, 2, 3, 4 and 5", joined);
    }

    [Fact]
    public void Joins_array_of_double()
    {
        double[] values = [1.7895, 2.84, 3.0, 4.44875, 5.555];
        var joined = StringHelper.Join(", ", values, " and ", "0.00", CultureInfo.InvariantCulture);
        Assert.Equal("1.79, 2.84, 3.00, 4.45 and 5.56", joined);
    }
}
