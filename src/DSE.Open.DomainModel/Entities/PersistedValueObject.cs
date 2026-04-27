// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base record type for a value object whose state is persisted as part of an
/// owning entity (e.g. an EF Core owned-type complex value).
/// </summary>
public abstract record PersistedValueObject;
