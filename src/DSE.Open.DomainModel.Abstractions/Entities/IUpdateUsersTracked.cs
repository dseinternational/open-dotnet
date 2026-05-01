// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Extends <see cref="IUpdatesTracked"/> with audit attribution — identifiers
/// for the users who created and last updated the object.
/// </summary>
public interface IUpdateUsersTracked : IUpdatesTracked
{
    /// <summary>
    /// An identifier for the user who created the object, or <see langword="null"/>
    /// if unknown or not yet persisted.
    /// </summary>
    string? CreatedUser { get; }

    /// <summary>
    /// An identifier for the user who most recently updated the object, or
    /// <see langword="null"/> if unknown or not yet persisted.
    /// </summary>
    string? UpdatedUser { get; }

    /// <summary>
    /// Stamps the <see cref="CreatedUser"/> and (initially) <see cref="UpdatedUser"/>
    /// identifiers when the entity is being created. Intended to be called by the
    /// persistence layer. Throws <see cref="InvalidOperationException"/> if
    /// <see cref="CreatedUser"/> has already been set.
    /// </summary>
    /// <param name="user">The non-empty user identifier to assign.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetCreatedUser(string user);

    /// <summary>
    /// Stamps the <see cref="UpdatedUser"/> identifier when the entity is being
    /// updated. Intended to be called by the persistence layer. Throws
    /// <see cref="InvalidOperationException"/> if <see cref="CreatedUser"/> has
    /// not yet been set.
    /// </summary>
    /// <param name="user">The non-empty user identifier to assign.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetUpdatedUser(string user);
}
