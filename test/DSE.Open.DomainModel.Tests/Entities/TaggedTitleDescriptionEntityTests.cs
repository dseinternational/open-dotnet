// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TaggedTitleDescriptionEntityTests
{
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_PopulatesAll()
    {
        var entity = new Fake(s_tag, "Hello", "World");
        Assert.Equal(s_tag, entity.Tag);
        Assert.Equal("Hello", entity.Title);
        Assert.Equal("World", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(s_tag, "t", null!));
    }

    [Fact]
    public void Implements_ITitled_IDescribed()
    {
        var entity = new Fake(s_tag, "t", "d");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
    }

    private sealed class Fake : TaggedTitleDescriptionEntity<Guid>
    {
        public Fake(Tag tag, string title, string description) : base(tag, title, description)
        {
        }
    }
}
