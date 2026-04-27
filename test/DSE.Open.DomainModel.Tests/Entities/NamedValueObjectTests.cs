// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class NamedValueObjectTests
{
    [Fact]
    public void Init_RequiresName()
    {
        var v = new Fake { Name = "widget" };
        Assert.Equal("widget", v.Name);
    }

    [Fact]
    public void Records_AreEqualByValue()
    {
        var a = new Fake { Name = "widget" };
        var b = new Fake { Name = "widget" };

        Assert.Equal(a, b);
    }

    [Fact]
    public void Records_AreNotEqualWhenNamesDiffer()
    {
        var a = new Fake { Name = "widget" };
        var b = new Fake { Name = "other" };

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Implements_INamed()
    {
        var v = new Fake { Name = "widget" };
        Assert.IsAssignableFrom<INamed>(v);
    }

    [Fact]
    public void Inherits_PersistedValueObject()
    {
        var v = new Fake { Name = "widget" };
        Assert.IsAssignableFrom<PersistedValueObject>(v);
    }

    private sealed record Fake : NamedValueObject;
}
