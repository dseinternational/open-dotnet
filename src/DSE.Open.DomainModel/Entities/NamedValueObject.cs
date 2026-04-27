// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A <see cref="PersistedValueObject"/> that exposes a required <see cref="Name"/>.
/// </summary>
public abstract record NamedValueObject : PersistedValueObject, INamed
{
    /// <inheritdoc />
    public virtual required string Name { get; init; }
}
