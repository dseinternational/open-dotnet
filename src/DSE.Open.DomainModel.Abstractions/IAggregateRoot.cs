// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Abstractions;

public interface IAggregateRoot : IEntity
{
}

public interface IAggregateRoot<TId> : IAggregateRoot, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
}
