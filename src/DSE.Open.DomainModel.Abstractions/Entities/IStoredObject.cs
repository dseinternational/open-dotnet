// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An object whose lifecycle state — newly created versus materialized from
/// storage — is exposed via <see cref="Initialization"/>.
/// </summary>
public interface IStoredObject
{
    /// <summary>
    /// Indicates whether this instance was newly created by the domain or
    /// reconstituted from storage.
    /// </summary>
    StoredObjectInitialization Initialization { get; }
}
