// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base exception thrown when entity data loaded from the data store is invalid
/// or inconsistent with the entity's invariants.
/// </summary>
public class EntityDataException : Exception
{
    /// <summary>
    /// Initializes a new <see cref="EntityDataException"/>.
    /// </summary>
    public EntityDataException()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="EntityDataException"/> with the specified message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityDataException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="EntityDataException"/> with the specified
    /// message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused this one.</param>
    public EntityDataException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
