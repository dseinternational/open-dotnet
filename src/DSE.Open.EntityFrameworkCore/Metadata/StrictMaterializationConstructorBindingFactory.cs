// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Reflection;
using DSE.Open.DomainModel.Abstractions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DSE.Open.EntityFrameworkCore.Metadata;

/// <summary>
/// A constructor binding factory that binds to a constructor marked with the
/// <see cref="MaterializationConstructorAttribute"/>. There must only be one
/// marked constructor and it must be bindable given the entity model.
/// </summary>
public class StrictMaterializationConstructorBindingFactory : ConstructorBindingFactoryBase
{
    public StrictMaterializationConstructorBindingFactory(
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

        // Used for join entity types
        // https://learn.microsoft.com/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many

        if (entityType.IsPropertyBag)
        {
            GetBindingsDefault(entityType, bind, out constructorBinding, out serviceOnlyBinding);
            return;
        }

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
                out var binding, out var failures))
            {
                constructorBinding = binding;
                serviceOnlyBinding = null;
                return;
            }
            else
            {
                var errorMessage = $"Failed to bind to a constructor marked with a " +
                    $"{nameof(MaterializationConstructorAttribute)} on entity type '{entityType.DisplayName()}'.";

                if (failures is not null)
                {
                    var constructorErrors = failures
                    .GroupBy(f => (ConstructorInfo)f.Member)
                    .Select(x => "    "
                        + CoreStrings.ConstructorBindingFailed(string.Join("', '", x.Select(f => f.Name)),
                        $"{entityType.DisplayName()}({string.Join(", ", ConstructConstructor(x))})")
                    );

                    static IEnumerable<string> ConstructConstructor(IGrouping<ConstructorInfo, ParameterInfo> parameters)
                    {
                        return parameters.Key.GetParameters()
                            .Select(y => $"{y.ParameterType.ShortDisplayName()} {y.Name}");
                    }

                    throw new InvalidOperationException(
                        errorMessage + Environment.NewLine +
                        string.Join(Environment.NewLine, constructorErrors) + Environment.NewLine);
                }

                throw new InvalidOperationException(errorMessage);
            }
        }

        throw new InvalidOperationException($"No constructor marked with a " +
            $"{nameof(MaterializationConstructorAttribute)} was found on entity type '{entityType.DisplayName()}'. " +
            $"When using {nameof(StrictMaterializationConstructorBindingFactory)}, all entities must have a " +
            $"constructor marked with a {nameof(MaterializationConstructorAttribute)}.");
    }
}
