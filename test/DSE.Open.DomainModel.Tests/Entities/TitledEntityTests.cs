// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TitledEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_WithTitle_PopulatesTitle()
    {
        var entity = new TitledFake("Hello");
        Assert.Equal("Hello", entity.Title);
    }

    [Fact]
    public void Ctor_NullTitle_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new TitledFake(null!));
    }

    [Fact]
    public void Ctor_WhitespaceTitle_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new TitledFake(" "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = DateTimeOffset.UtcNow;

        var entity = new TitledFake(id, "Hello", created, updated, s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("Hello", entity.Title);
    }

    [Fact]
    public void Title_Setter_UpdatesValue()
    {
        var entity = new TitledFake("a") { Title = "b" };
        Assert.Equal("b", entity.Title);
    }

    [Fact]
    public void Implements_ITitled()
    {
        Assert.IsAssignableFrom<ITitled>(new TitledFake("t"));
    }

    private sealed class TitledFake : TitledEntity<Guid>
    {
        public TitledFake(string title) : base(title)
        {
        }

        public TitledFake(Guid id, string title) : base(id, title)
        {
        }

        public TitledFake(Guid id, string title, DateTimeOffset? created, DateTimeOffset? updated, Timestamp? timestamp)
            : base(id, title, created, updated, timestamp)
        {
        }
    }
}
