// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An entity that has no independent identity outside its owning aggregate and
/// exposes the identifier of its parent.
/// </summary>
/// <typeparam name="TParentId">The parent's identifier type.</typeparam>
public interface IOwnedEntity<TParentId>
    where TParentId : struct, IEquatable<TParentId>
{
    /// <summary>
    /// Gets the id of the parent entity that owns this entity.
    /// </summary>
    TParentId ParentId { get; }
}
