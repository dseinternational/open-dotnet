// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TaggedTitledEntityTests
{
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_PopulatesTagAndTitle()
    {
        var entity = new Fake(s_tag, "Hello");
        Assert.Equal(s_tag, entity.Tag);
        Assert.Equal("Hello", entity.Title);
    }

    [Fact]
    public void Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(default(Tag), "Hello"));
    }

    [Fact]
    public void Implements_ITitled()
    {
        Assert.IsAssignableFrom<ITitled>(new Fake(s_tag, "t"));
    }

    private sealed class Fake : TaggedTitledEntity<Guid>
    {
        public Fake(Tag tag, string title) : base(tag, title)
        {
        }
    }
}
