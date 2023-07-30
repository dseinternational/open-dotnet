// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DSE.Open.EntityFrameworkCore.Metadata;

public abstract class ConstructorBindingFactoryBase : IConstructorBindingFactory
{
    private readonly IPropertyParameterBindingFactory _propertyFactory;
    private readonly IParameterBindingFactories _factories;

    protected ConstructorBindingFactoryBase(
        IPropertyParameterBindingFactory propertyFactory,
        IParameterBindingFactories factories)
    {
        _propertyFactory = propertyFactory;
        _factories = factories;
    }

    public IPropertyParameterBindingFactory PropertyFactory => _propertyFactory;

    public IParameterBindingFactories Factories => _factories;

    public virtual void GetBindings(
        IConventionEntityType entityType,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding)
    {
        GetBindings(
            entityType,
            static (f, e, p, n) => f?.Bind((IConventionEntityType)e, p, n),
            out constructorBinding,
            out serviceOnlyBinding);
    }

    public virtual void GetBindings(
        IMutableEntityType entityType,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding)
    {
        GetBindings(
            entityType,
            static (f, e, p, n) => f?.Bind((IMutableEntityType)e, p, n),
            out constructorBinding,
            out serviceOnlyBinding);
    }

    public virtual void GetBindings(
        IReadOnlyEntityType entityType,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding)
    {
        GetBindings(
            entityType,
            static (f, e, p, n) => f?.Bind(e, p, n),
            out constructorBinding,
            out serviceOnlyBinding);
    }

    protected abstract void GetBindings(
        IReadOnlyEntityType entityType,
        Func<IParameterBindingFactory?, IReadOnlyEntityType, Type, string, ParameterBinding?> bind,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding);

    public virtual bool TryBindConstructor(
        IMutableEntityType entityType,
        ConstructorInfo constructor,
        [NotNullWhen(true)] out InstantiationBinding? binding,
        [NotNullWhen(false)] out IEnumerable<ParameterInfo>? unboundParameters)
    {
        return TryBindConstructor(
            entityType,
            constructor,
            static (f, e, p, n) => f?.Bind((IMutableEntityType)e, p, n),
            out binding,
            out unboundParameters);
    }

    public virtual bool TryBindConstructor(
        IConventionEntityType entityType,
        ConstructorInfo constructor,
        [NotNullWhen(true)] out InstantiationBinding? binding,
        [NotNullWhen(false)] out IEnumerable<ParameterInfo>? unboundParameters)
    {
        return TryBindConstructor(
            entityType,
            constructor,
            static (f, e, p, n) => f?.Bind((IConventionEntityType)e, p, n),
            out binding,
            out unboundParameters);
    }

    protected virtual bool TryBindConstructor(
        IReadOnlyEntityType entityType,
        ConstructorInfo constructor,
        Func<IParameterBindingFactory?, IReadOnlyEntityType, Type, string, ParameterBinding?> bind,
        [NotNullWhen(true)] out InstantiationBinding? binding,
        [NotNullWhen(false)] out IEnumerable<ParameterInfo>? unboundParameters)
    {
        Guard.IsNotNull(constructor);

        IEnumerable<(ParameterInfo Parameter, ParameterBinding? Binding)> bindings
            = constructor.GetParameters().Select(
                    p => (p, string.IsNullOrEmpty(p.Name)
                        ? null
                        : PropertyFactory.FindParameter((IEntityType)entityType, p.ParameterType, p.Name)
                        ?? bind(Factories.FindFactory(p.ParameterType, p.Name), entityType, p.ParameterType, p.Name)))
                .ToList();

        if (bindings.Any(b => b.Binding == null))
        {
            unboundParameters = bindings.Where(b => b.Binding == null).Select(b => b.Parameter);
            binding = null;

            return false;
        }

        unboundParameters = null;
        binding = new ConstructorBinding(constructor, bindings.Select(b => b.Binding).ToList()!);

        return true;
    }

    protected static string FormatConstructorString(IReadOnlyEntityType entityType, InstantiationBinding binding)
    {
        Guard.IsNotNull(entityType);
        Guard.IsNotNull(binding);

        return entityType.ClrType.ShortDisplayName()
                + "("
                + string.Join(", ", binding.ParameterBindings.Select(b => b.ParameterType.ShortDisplayName()))
                + ")";
    }

    protected void GetBindingsDefault(
        IReadOnlyEntityType entityType,
        Func<IParameterBindingFactory?, IReadOnlyEntityType, Type, string, ParameterBinding?> bind,
        out InstantiationBinding constructorBinding,
        out InstantiationBinding? serviceOnlyBinding)
    {
        Guard.IsNotNull(entityType);

        var maxServiceParams = 0;
        var maxServiceOnlyParams = 0;
        var minPropertyParams = int.MaxValue;
        var foundBindings = new List<InstantiationBinding>();
        var foundServiceOnlyBindings = new List<InstantiationBinding>();
        var bindingFailures = new List<IEnumerable<ParameterInfo>>();

        foreach (var constructor in entityType.ClrType.GetTypeInfo()
            .DeclaredConstructors
            .Where(c => !c.IsStatic))
        {
            if (TryBindConstructor(entityType, constructor, bind, out var binding, out var failures))
            {
                var serviceParamCount = binding.ParameterBindings.OfType<ServiceParameterBinding>().Count();
                var propertyParamCount = binding.ParameterBindings.Count - serviceParamCount;

                if (propertyParamCount == 0)
                {
                    if (serviceParamCount == maxServiceOnlyParams)
                    {
                        foundServiceOnlyBindings.Add(binding);
                    }
                    else if (serviceParamCount > maxServiceOnlyParams)
                    {
                        foundServiceOnlyBindings.Clear();
                        foundServiceOnlyBindings.Add(binding);

                        maxServiceOnlyParams = serviceParamCount;
                    }
                }

                if (serviceParamCount == maxServiceParams && propertyParamCount == minPropertyParams)
                {
                    foundBindings.Add(binding);
                }
                else if (serviceParamCount > maxServiceParams)
                {
                    foundBindings.Clear();
                    foundBindings.Add(binding);

                    maxServiceParams = serviceParamCount;
                    minPropertyParams = propertyParamCount;
                }
                else if (propertyParamCount < minPropertyParams)
                {
                    foundBindings.Clear();
                    foundBindings.Add(binding);

                    maxServiceParams = serviceParamCount;
                    minPropertyParams = propertyParamCount;
                }
            }
            else
            {
                bindingFailures.Add(failures);
            }
        }

        if (foundBindings.Count == 0)
        {
            var constructorErrors = bindingFailures.SelectMany(f => f)
                .GroupBy(f => (ConstructorInfo)f.Member)
                .Select(
                    x => "    "
                        + CoreStrings.ConstructorBindingFailed(
                            string.Join("', '", x.Select(f => f.Name)),
                            $"{entityType.DisplayName()}({string.Join(", ", ConstructConstructor(x))})")
                );

            static IEnumerable<string> ConstructConstructor(IGrouping<ConstructorInfo, ParameterInfo> parameters)
            {
                return parameters.Key.GetParameters().Select(y => $"{y.ParameterType.ShortDisplayName()} {y.Name}");
            }

            throw new InvalidOperationException(
                CoreStrings.ConstructorNotFound(
                    entityType.DisplayName(),
                    Environment.NewLine + string.Join(Environment.NewLine, constructorErrors) + Environment.NewLine));
        }

        if (foundBindings.Count > 1)
        {
            throw new InvalidOperationException(
                CoreStrings.ConstructorConflict(
                    FormatConstructorString(entityType, foundBindings[0]),
                    FormatConstructorString(entityType, foundBindings[1])));
        }

        constructorBinding = foundBindings[0];
        serviceOnlyBinding = foundServiceOnlyBindings.Count == 1 ? foundServiceOnlyBindings[0] : null;
    }

    protected static class CoreStrings
    {
        public static string ConstructorBindingFailed(object? failedBinds, object? parameters) => $"Cannot bind '{failedBinds}' in '{parameters}'";

        public static string ConstructorConflict(object? firstConstructor, object? secondConstructor)
        {
            return $"The constructors '{firstConstructor}' and '{secondConstructor}' have the same number " +
                "of parameters, and can both be used by Entity Framework. The constructor to be used must " +
                "be configured in 'OnModelCreating'.";
        }

        public static string ConstructorNotFound(object? entityType, object? constructors)
        {
            return $"No suitable constructor was found for entity type '{entityType}'. The following " +
                "constructors had parameters that could not be bound to properties of the entity type: " +
                $"{constructors} Note that only mapped properties can be bound to constructor parameters. " +
                "Navigations to related entities, including references to owned types, cannot be bound.";
        }
    }
}
