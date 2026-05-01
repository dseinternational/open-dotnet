// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedTaggedTitledEntityTests
{
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_PopulatesTagAndTitle()
    {
        var entity = new Fake(s_tag, "Widget");
        Assert.Equal(s_tag, entity.Tag);
        Assert.Equal("Widget", entity.Title);
    }

    [Fact]
    public void Ctor_NullTitle_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(s_tag, null!));
    }

    [Fact]
    public void Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(default, "Widget"));
    }

    [Fact]
    public void Implements_ITitled_And_IUpdateUsersTracked()
    {
        var entity = new Fake(s_tag, "Widget");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
    }

    private sealed class Fake : UpdateUsersTrackedTaggedTitledEntity<Guid>
    {
        public Fake(Tag tag, string title) : base(tag, title)
        {
        }
    }
}
