// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Represents an object that is identified by the value of an Id property.
/// </summary>
public interface IIdentified
{
    /// <summary>Gets a value that identifies the current instance.</summary>
    object Id { get; }
}

/// <summary>
/// Represents an object that is identified by the value of an Id property.
/// </summary>
/// <typeparam name="TId">The type of the Id property</typeparam>
public interface IIdentified<out TId> : IIdentified
    where TId : struct, IEquatable<TId>
{
    /// <summary>Gets a value that identifies the current instance.</summary>
    new TId Id { get; }
}
