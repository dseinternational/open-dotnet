// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Marks the constructor on a concrete, derived domain entity or stored object that
/// an ORM should use to reconstitute the instance from the data store.
/// </summary>
/// <remarks>
/// <para>
/// Exactly one constructor per concrete entity type should carry this attribute.
/// The marked constructor must chain to the base class's materialization
/// constructor — the one that sets
/// <see cref="IStoredObject.Initialization"/> to
/// <see cref="StoredObjectInitialization.Materialized"/> and accepts the full set
/// of stored values (identifier, timestamps, concurrency token, etc.). It must
/// not chain to a base parameterless constructor: the parameterless constructors
/// on the abstract bases set
/// <see cref="IStoredObject.Initialization"/> to
/// <see cref="StoredObjectInitialization.Created"/> and leave identifiers and
/// timestamps at their defaults, which would misrepresent a reconstituted
/// instance as newly created.
/// </para>
/// <para>
/// The DSE EF Core integration discovers marked constructors via
/// <c>MaterializationConstructorBindingFactory</c> /
/// <c>StrictMaterializationConstructorBindingFactory</c>. With the strict factory,
/// every entity type must declare a marked constructor; with the non-strict factory,
/// entities without one fall back to EF Core's default binding heuristic, which may
/// silently pick the parameterless base-class path — therefore deriving from the
/// abstract stored-object / entity bases without declaring a marked materialization
/// constructor is not supported.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Constructor)]
public sealed class MaterializationConstructorAttribute : Attribute;
