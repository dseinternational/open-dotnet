// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Abstractions;

namespace DSE.Open.DomainModel.Entities;

public abstract class TimestampedStoredObject : StoredObject, ITimestamped
{
    private readonly Timestamp? _timestamp;

    protected TimestampedStoredObject()
        : base(StoredObjectInitialization.Created)
    {
    }

    protected TimestampedStoredObject(Timestamp? timestamp)
        : base(StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(timestamp is null || timestamp.Value == default);

        _timestamp = timestamp;
    }

    public Timestamp? Timestamp => _timestamp;
}
