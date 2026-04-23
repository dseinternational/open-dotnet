// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Marks an entity as the root of an aggregate — the only type in the aggregate
/// that external callers may reference directly. Aggregate roots own the
/// consistency boundary for the entities they contain.
/// </summary>
public interface IAggregateRoot : IEntity;

/// <summary>
/// An <see cref="IAggregateRoot"/> with a strongly-typed identifier.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public interface IAggregateRoot<TId> : IAggregateRoot, IEntity<TId>
    where TId : struct, IEquatable<TId>;
