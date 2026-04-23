// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdatesTrackedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Parameterless_Ctor_LeavesFieldsUnset()
    {
        var entity = new UpdatesTrackedFake();

        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
        Assert.Null(entity.Timestamp);
    }

    [Fact]
    public void Materialization_Ctor_Populates_All_Fields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var updated = DateTimeOffset.Parse("2024-06-01T12:00:00Z", CultureInfo.InvariantCulture);

        var entity = new UpdatesTrackedFake(id, created, updated, s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(id, entity.Id);
        Assert.Equal(created, entity.Created);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
    }

    [Fact]
    public void Materialization_Ctor_NullTimestamp_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new UpdatesTrackedFake(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                null));
    }

    [Fact]
    public void Materialization_Ctor_DefaultTimestamp_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new UpdatesTrackedFake(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                default(Timestamp)));
    }

    [Fact]
    public void SetCreated_SetsBothTimestamps()
    {
        var clock = new FakeTimeProvider();
        IUpdateTimesTracked entity = new UpdatesTrackedFake();

        entity.SetCreated(clock);

        Assert.NotNull(entity.Created);
        Assert.Equal(entity.Created, entity.Updated);
    }

    [Fact]
    public void IsPersisted_ReturnsTrueForMaterializedEntity()
    {
        var entity = new UpdatesTrackedFake(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            s_sampleTimestamp);

        Assert.True(((ITimestamped)entity).IsPersisted());
    }

    [Fact]
    public void IsPersisted_ReturnsFalseForNewEntity()
    {
        Assert.False(((ITimestamped)new UpdatesTrackedFake()).IsPersisted());
    }

    private sealed class UpdatesTrackedFake : UpdatesTrackedEntity<Guid>
    {
        public UpdatesTrackedFake()
        {
        }

        public UpdatesTrackedFake(Guid id, DateTimeOffset? created, DateTimeOffset? updated, Timestamp? timestamp)
            : base(id, created, updated, timestamp)
        {
        }
    }
}
