// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace DSE.Open.DomainModel.Entities;

public abstract class StoredObject : IStoredObject
{
    protected StoredObject(StoredObjectInitialization initialization)
    {
        Initialization = initialization;
    }

    [NotMapped]
    public StoredObjectInitialization Initialization { get; }
}
