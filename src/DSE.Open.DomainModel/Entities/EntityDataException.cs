// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

public class EntityDataException : Exception
{
    public EntityDataException()
    {
    }

    public EntityDataException(string? message) : base(message)
    {
    }

    public EntityDataException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
