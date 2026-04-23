// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
}
