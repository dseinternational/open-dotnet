// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TaggedNamedEntityTests
{
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_PopulatesTagAndName()
    {
        var entity = new Fake(s_tag, "Widget");
        Assert.Equal(s_tag, entity.Tag);
        Assert.Equal("Widget", entity.Name);
    }

    [Fact]
    public void Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(default(Tag), "Widget"));
    }

    [Fact]
    public void Materialization_Ctor_DefaultTag_Throws()
    {
        var ts = new Timestamp([1, 2, 3, 4, 5, 6, 7, 8]);
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), default(Tag), "Widget", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, ts));
    }

    [Fact]
    public void Implements_INamed()
    {
        Assert.IsAssignableFrom<INamed>(new Fake(s_tag, "n"));
    }

    private sealed class Fake : TaggedNamedEntity<Guid>
    {
        public Fake(Tag tag, string name) : base(tag, name)
        {
        }

        public Fake(Guid id, Tag tag, string name, DateTimeOffset? created, DateTimeOffset? updated, Timestamp? timestamp)
            : base(id, tag, name, created, updated, timestamp)
        {
        }
    }
}
