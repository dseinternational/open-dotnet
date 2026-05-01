// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="IEntity"/> that also exposes a globally-unique
/// <see cref="IUniquelyIdentified.UniqueId"/> identifier in addition to its
/// primary key.
/// </summary>
public interface IUniquelyIdentifiedEntity : IEntity, IUniquelyIdentified;

/// <summary>
/// A uniquely-identified entity with a strongly-typed primary key.
/// </summary>
/// <typeparam name="TId">The primary key value type.</typeparam>
public interface IUniquelyIdentifiedEntity<TId> : IEntity<TId>, IUniquelyIdentifiedEntity
    where TId : struct, IEquatable<TId>;
