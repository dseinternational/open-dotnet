// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Abstractions;
using DSE.Open.Events.Abstractions;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Events;

/// <summary>
/// Base implementation for an object that describes something that happened in the domain
/// that other parts of the same domain (in-process) may choose to respond to.
/// </summary>
public abstract class DomainEvent<TData> : IDomainEvent<TData>
{
    private Identifier _id;

    protected DomainEvent(TData data)
    {
        Guard.IsNotNull(data);

        Source = EventSourceConfiguration.GetEventSource(GetType()) ??
            throw new InvalidOperationException("Source must be provided.");

        Data = data;
    }

    /// <summary>
    /// Gets the time the event was created.
    /// </summary>
    public DateTimeOffset Time { get; protected set; } = DateTimeOffset.Now;

    /// <summary>
    /// The data associated with the event.
    /// </summary>
    public virtual TData Data { get; }

    /// <inheritdoc />
    object? IEvent.Data => Data;

    /// <inheritdoc />
    public virtual Identifier Id => _id != default ? _id : (_id = Identifier.New("dse_evt"));

    /// <inheritdoc />
    public Uri Source { get; }

    /// <inheritdoc />
    public string? Subject { get; init; }

    /// <inheritdoc />
    public abstract string Type { get; }
}
