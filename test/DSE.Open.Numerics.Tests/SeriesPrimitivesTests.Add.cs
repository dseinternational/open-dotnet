// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void Add_Int32()
    {
        var series = Series.Create([1, 2, 3, 4, 5], "series1");
        var addend = Series.Create([1, 2, 3, 4, 5], "series1");

        var result = series.Add(addend);

        Assert.Equal(2, result[0]);
        Assert.Equal(4, result[1]);
        Assert.Equal(6, result[2]);
        Assert.Equal(8, result[3]);
        Assert.Equal(10, result[4]);
        Assert.Null(result.Name);
    }

    [Fact]
    public void Add_SeriesSeries_IntoAliasedRhsSpan_WritesSum()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([4, 5, 6]);

        lhs.Add(rhs, rhs.AsSpan());

        Assert.Equal(5, rhs[0]);
        Assert.Equal(7, rhs[1]);
        Assert.Equal(9, rhs[2]);
    }
}
