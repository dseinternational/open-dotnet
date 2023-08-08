// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

public enum StoredObjectInitialization
{
    /// <summary>
    /// Indicates that the object was created as a 'new' object.
    /// </summary>
    Created,

    /// <summary>
    /// Indicates that the object was materialized from storage as an 'existing' object.
    /// </summary>
    Materialized
}
