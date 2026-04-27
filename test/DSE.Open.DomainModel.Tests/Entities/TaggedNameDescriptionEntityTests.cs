// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TaggedNameDescriptionEntityTests
{
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_PopulatesAll()
    {
        var entity = new Fake(s_tag, "Widget", "A small widget");
        Assert.Equal(s_tag, entity.Tag);
        Assert.Equal("Widget", entity.Name);
        Assert.Equal("A small widget", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(s_tag, "n", null!));
    }

    [Fact]
    public void Implements_INamed_IDescribed()
    {
        var entity = new Fake(s_tag, "n", "d");
        Assert.IsAssignableFrom<INamed>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
    }

    private sealed class Fake : TaggedNameDescriptionEntity<Guid>
    {
        public Fake(Tag tag, string name, string description) : base(tag, name, description)
        {
        }
    }
}
