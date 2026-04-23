// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A domain entity — a <see cref="IStoredObject"/> that carries an identifier.
/// </summary>
public interface IEntity : IStoredObject, IIdentified;

/// <summary>
/// A domain entity with a strongly-typed identifier.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public interface IEntity<TId> : IEntity, IIdentified<TId>
    where TId : struct, IEquatable<TId>;
