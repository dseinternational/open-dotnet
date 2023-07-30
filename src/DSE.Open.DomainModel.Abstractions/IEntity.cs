// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Abstractions;

public interface IEntity : IStoredObject, IIdentified
{
}

public interface IEntity<TId> : IEntity, IIdentified<TId>
    where TId : struct, IEquatable<TId>
{
}
