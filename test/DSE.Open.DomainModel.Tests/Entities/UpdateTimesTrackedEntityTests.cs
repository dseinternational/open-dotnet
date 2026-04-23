// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateTimesTrackedEntityTests
{
    [Fact]
    public void Parameterless_Ctor_LeavesTimestampsUnset()
    {
        var entity = new UpdateTimesTrackedFake();

        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void Id_Ctor_LeavesTimestampsUnset()
    {
        var id = Guid.NewGuid();
        var entity = new UpdateTimesTrackedFake(id);

        Assert.Equal(id, entity.Id);
        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
    }

    [Fact]
    public void Materialization_Ctor_Populates_Created_And_Updated()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var updated = DateTimeOffset.Parse("2024-06-01T12:00:00Z", CultureInfo.InvariantCulture);

        var entity = new UpdateTimesTrackedFake(id, created, updated);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(created, entity.Created);
        Assert.Equal(updated, entity.Updated);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Materialization_Ctor_NullTimestamps_Throws(bool createdNull, bool updatedNull)
    {
        DateTimeOffset? created = createdNull ? null : DateTimeOffset.UtcNow;
        DateTimeOffset? updated = updatedNull ? null : DateTimeOffset.UtcNow;

        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new UpdateTimesTrackedFake(Guid.NewGuid(), created, updated));
    }

    [Fact]
    public void SetCreated_UsingSuppliedTimeProvider_SetsBothTimestamps()
    {
        var now = DateTimeOffset.Parse("2024-03-15T10:00:00Z", CultureInfo.InvariantCulture);
        var clock = new FakeTimeProvider(now);

        IUpdateTimesTracked entity = new UpdateTimesTrackedFake();
        entity.SetCreated(clock);

        Assert.NotNull(entity.Created);
        Assert.Equal(entity.Created, entity.Updated);
    }

    [Fact]
    public void SetCreated_CalledTwice_Throws()
    {
        var clock = new FakeTimeProvider();
        IUpdateTimesTracked entity = new UpdateTimesTrackedFake();
        entity.SetCreated(clock);

        _ = Assert.Throws<InvalidOperationException>(() => entity.SetCreated(clock));
    }

    [Fact]
    public void SetUpdated_BeforeSetCreated_Throws()
    {
        var clock = new FakeTimeProvider();
        IUpdateTimesTracked entity = new UpdateTimesTrackedFake();

        _ = Assert.Throws<InvalidOperationException>(() => entity.SetUpdated(clock));
    }

    [Fact]
    public void SetUpdated_AfterSetCreated_MovesUpdatedForward()
    {
        var clock = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture));

        IUpdateTimesTracked entity = new UpdateTimesTrackedFake();
        entity.SetCreated(clock);
        var created = entity.Created;

        clock.Advance(TimeSpan.FromHours(1));
        entity.SetUpdated(clock);

        Assert.Equal(created, entity.Created);
        Assert.True(entity.Updated > entity.Created);
    }

    [Fact]
    public void SetCreated_NullTimeProvider_FallsBackToSystemClock()
    {
        IUpdateTimesTracked entity = new UpdateTimesTrackedFake();
        entity.SetCreated(null);

        Assert.NotNull(entity.Created);
    }

    private sealed class UpdateTimesTrackedFake : UpdateTimesTrackedEntity<Guid>
    {
        public UpdateTimesTrackedFake()
        {
        }

        public UpdateTimesTrackedFake(Guid id) : base(id)
        {
        }

        public UpdateTimesTrackedFake(Guid id, DateTimeOffset? created, DateTimeOffset? updated)
            : base(id, created, updated)
        {
        }
    }
}
