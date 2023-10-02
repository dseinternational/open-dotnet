// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using FluentAssertions;

namespace DSE.Open.Results.Tests;

public class PaginationTests
{
    public PaginationTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void Init()
    {
        var p = new Pagination(111, 20, 1);
        Assert.Equal(6, p.TotalPages);
        Assert.Equal(2, p.NextPage);
        Assert.Null(p.PreviousPage);
        Assert.Equal("1-20 of 111", p.GetDescription());
    }

    [Fact]
    public void Init_2()
    {
        var p = new Pagination(111, 20, 4);
        Assert.Equal(6, p.TotalPages);
        Assert.Equal(5, p.NextPage);
        Assert.Equal(3, p.PreviousPage);
        Assert.Equal("61-80 of 111", p.GetDescription());
    }

    [Fact]
    public void Init_3()
    {
        var p = new Pagination(9, 20, 1);
        Assert.Equal(1, p.TotalPages);
        Assert.Null(p.NextPage);
        Assert.Null(p.PreviousPage);
        Assert.Equal("1-9 of 9", p.GetDescription());
    }

    [Fact]
    public void Init_4()
    {
        var p = new Pagination(20, 20, 1);
        Assert.Equal(1, p.TotalPages);
        Assert.Null(p.NextPage);
        Assert.Null(p.PreviousPage);
        Assert.Equal("1-20 of 20", p.GetDescription());
    }

    [Fact]
    public void Init_5()
    {
        var p = new Pagination(0, 20, 1);
        Assert.Equal(0, p.TotalPages);
        Assert.Null(p.NextPage);
        Assert.Null(p.PreviousPage);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var p = new Pagination(111, 20, 4);

        var json = JsonSerializer.Serialize(p);

        Output.WriteLine(json);

        var p2 = JsonSerializer.Deserialize<Pagination>(json);

        _ = p2.Should().BeEquivalentTo(p, config => config.ComparingByMembers<Pagination>());
    }
}
