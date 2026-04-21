// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base class for persistable objects whose lifecycle state (newly created versus
/// materialized from storage) is tracked via <see cref="Initialization"/>.
/// </summary>
/// <remarks>
/// <para>
/// Derived classes in this hierarchy expose two kinds of constructor — a
/// domain-facing constructor for creating a brand-new instance (which chains to a
/// base constructor passing <see cref="StoredObjectInitialization.Created"/>) and
/// a materialization constructor that an ORM uses to reconstitute an existing
/// instance from the data store (which chains to a base constructor passing
/// <see cref="StoredObjectInitialization.Materialized"/>).
/// </para>
/// <para>
/// Concrete, derived entity types must declare their own materialization
/// constructor, mark it with
/// <see cref="MaterializationConstructorAttribute"/>, and chain it to the
/// materialization-state constructor on their base class. The parameterless
/// constructors on the abstract base classes set
/// <see cref="Initialization"/> to <see cref="StoredObjectInitialization.Created"/>
/// and leave identifiers/timestamps at their defaults, so they must never be
/// used as the materialization path — see
/// <see cref="MaterializationConstructorAttribute"/> for details.
/// </para>
/// </remarks>
public abstract class StoredObject : IStoredObject
{
    /// <param name="initialization">
    /// Pass <see cref="StoredObjectInitialization.Created"/> when the object is being
    /// newly created by the domain, and <see cref="StoredObjectInitialization.Materialized"/>
    /// when it is being reconstituted from storage. Callers are responsible for
    /// choosing the correct value so that <see cref="Initialization"/> accurately
    /// reflects the object's lifecycle state.
    /// </param>
    protected StoredObject(StoredObjectInitialization initialization)
    {
        Initialization = initialization;
    }

    [NotMapped]
    public StoredObjectInitialization Initialization { get; }
}
