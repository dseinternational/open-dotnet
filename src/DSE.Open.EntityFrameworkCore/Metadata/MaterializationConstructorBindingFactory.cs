// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Reflection;
using DSE.Open.DomainModel.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DSE.Open.EntityFrameworkCore.Metadata;

/// <summary>
/// A constructor binding factory that binds to a constructor marked with the
/// <see cref="MaterializationConstructorAttribute"/>. There must only be one
/// marked constructor and it must be bindable given the entity model. If no
/// constructor marked with the <see cref="MaterializationConstructorAttribute"/>
/// is found, falls back to the default behaviour of <see cref="ConstructorBindingFactory"/>.
/// </summary>
public class MaterializationConstructorBindingFactory : ConstructorBindingFactoryBase
{
    public MaterializationConstructorBindingFactory(
        IPropertyParameterBindingFactory propertyFactory,
        IParameterBindingFactories factories)
        : base(propertyFactory, factories)
    {
    }

    protected override void GetBindings(
        IReadOnlyEntityType entityType,
        Func<IParameterBindingFactory?, IReadOnlyEntityType, Type, string, ParameterBinding?> bind,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding)
    {
        Guard.IsNotNull(entityType);

        var constructorsWithAttribute = entityType.ClrType.GetTypeInfo()
            .DeclaredConstructors
            .Where(c => !c.IsStatic && c.GetCustomAttribute<MaterializationConstructorAttribute>() != null)
            .ToList();

        if (constructorsWithAttribute.Count > 1)
        {
            throw new InvalidOperationException("More than one constructor is marked with a " +
                $"{nameof(MaterializationConstructorAttribute)} on entity type '{entityType.DisplayName()}'. " +
                $"Only one constructor may be marked with a {nameof(MaterializationConstructorAttribute)}.");
        }
        else if (constructorsWithAttribute.Count == 1)
        {
            if (TryBindConstructor(entityType, constructorsWithAttribute[0], bind,
                out var binding, out _))
            {
                constructorBinding = binding;
                serviceOnlyBinding = null;
                return;
            }
            else
            {
                throw new InvalidOperationException($"Failed to bind to a constructor marked with a " +
                    $"{nameof(MaterializationConstructorAttribute)} on entity type '{entityType.DisplayName()}'.");
            }
        }

        GetBindingsDefault(entityType, bind, out constructorBinding, out serviceOnlyBinding);
    }
}
