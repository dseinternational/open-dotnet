// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="OwnedEntity"/> (no independent identity) that exposes a non-null,
/// non-whitespace <see cref="Name"/>.
/// </summary>
public abstract class NamedOwnedEntity : OwnedEntity, INamed
{
    private string _name;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="name"/>.
    /// </summary>
    protected NamedOwnedEntity(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected NamedOwnedEntity(string name, StoredObjectInitialization initialization)
        : base(initialization)
    {
        if (initialization == StoredObjectInitialization.Materialized)
        {
            EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(name));
        }
        else
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
        }

        _name = name;
    }

    /// <inheritdoc cref="INamed.Name" />
    public virtual string Name
    {
        get => _name;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _name = value;
        }
    }
}
