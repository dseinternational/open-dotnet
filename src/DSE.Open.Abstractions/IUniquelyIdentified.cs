// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Represents an object that has a stable, globally unique identifier.
/// </summary>
public interface IUniquelyIdentified
{
    /// <summary>
    /// Gets a value that uniquely identifies the current instance.
    /// </summary>
    Guid UniqueId { get; }
}
