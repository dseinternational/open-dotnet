// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An object that carries an opaque concurrency <see cref="Timestamp"/> used
/// by stores to detect conflicting updates.
/// </summary>
public interface ITimestamped
{
    /// <summary>
    /// The concurrency timestamp assigned by the data store, or
    /// <see langword="null"/> if the object has not been persisted.
    /// </summary>
    Timestamp? Timestamp { get; }
}
