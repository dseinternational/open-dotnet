// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Stored object that carries a concurrency <see cref="Timestamp"/>.
/// </summary>
/// <remarks>
/// See <see cref="StoredObject"/> for the constructor contract. The parameterless
/// constructor is the 'new object' path; the <c>(Timestamp?)</c> constructor is
/// the materialization path and derived concrete types must chain to it from a
/// constructor marked with <see cref="MaterializationConstructorAttribute"/>.
/// </remarks>
public abstract class TimestampedStoredObject : StoredObject, ITimestamped
{
    private Timestamp? _timestamp;

    /// <summary>
    /// Initializes a newly created stored object — <see cref="Timestamp"/> is unset
    /// and <see cref="StoredObject.Initialization"/> is
    /// <see cref="StoredObjectInitialization.Created"/>.
    /// </summary>
    protected TimestampedStoredObject()
        : base(StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the object from storage.
    /// </summary>
    /// <param name="timestamp">Concurrency timestamp loaded from storage. Must be non-null
    /// and non-default.</param>
    protected TimestampedStoredObject(Timestamp? timestamp)
        : base(StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(timestamp is null || timestamp.Value == default);

        _timestamp = timestamp;
    }

    /// <inheritdoc />
    public Timestamp? Timestamp => _timestamp;

    /// <summary>
    /// Sets the concurrency <see cref="Timestamp"/>. Intended for the persistence layer
    /// to call after a successful insert or update on data stores that do not auto-populate
    /// a row-version column (for example SQLite without rowversion support).
    /// </summary>
    /// <param name="value">The timestamp to assign. Must not be the default
    /// <see cref="Timestamp"/>.</param>
    protected void SetTimestamp(Timestamp value)
    {
        EntityDataInitializationException.ThrowIf(value == default);
        _timestamp = value;
    }
}
