// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using DSE.Open.Values.Generators.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DSE.Open.Values.Generators;

// See:
// - https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/gen
// - https://andrewlock.net/series/creating-a-source-generator/
// - https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/gen/JsonSourceGenerator.Emitter.cs
// - https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm.SourceGenerators
// - https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Mvvm.SourceGenerators/ComponentModel/ObservablePropertyGenerator.Execute.cs

[Generator]
public sealed partial class ValueTypesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if LAUNCH_DEBUGGER
        System.Diagnostics.Debugger.Launch();
#endif

        var valueTypeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxStructWithAttributes(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<StructDeclarationSyntax?>)> compilationAndValueTypes
            = context.CompilationProvider.Combine(valueTypeDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndValueTypes,
            static (spc, source) => Execute(source.Item1, source.Item2!, spc));
    }

    private static bool IsSyntaxStructWithAttributes(SyntaxNode node)
        => node is StructDeclarationSyntax cds && cds.AttributeLists.Count > 0;

    private static StructDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var structDeclarationSyntax = (StructDeclarationSyntax)context.Node;

        // TODO: Report error if more than one attribute applied

        foreach (var attributeListSyntax in structDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    continue;
                }

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;

                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName is TypeNames.NominalValueAttributeFullName
                    or TypeNames.IntervalValueAttributeFullName
                    or TypeNames.OrdinalValueAttributeFullName
                    or TypeNames.RatioValueAttributeFullName)
                {
                    return structDeclarationSyntax;
                }
            }
        }

        return default;
    }

    private static void Execute(
        Compilation compilation,
        ImmutableArray<StructDeclarationSyntax> structs,
        SourceProductionContext context)
    {
        if (structs.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        var distinctClasses = structs.Distinct();

        var diagnostics = new List<Diagnostic>();

        var valuesToGenerate = GetValueTypeSpecs(
            compilation,
            distinctClasses,
            diagnostics.Add,
            context.CancellationToken);

        foreach (var d in diagnostics)
        {
            context.ReportDiagnostic(d);
        }

        diagnostics.Clear();

        foreach (var value in valuesToGenerate)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var result = Emitter.GenerateValueStruct(value, diagnostics.Add);

            context.AddSource($"{value.ValueTypeName}.g.cs", result);

            foreach (var d in diagnostics)
            {
                context.ReportDiagnostic(d);
            }

            diagnostics.Clear();
        }
    }

    private static List<ValueTypeSpec> GetValueTypeSpecs(
        Compilation compilation,
        IEnumerable<StructDeclarationSyntax> structs,
        Action<Diagnostic> reportDiagnostic,
        CancellationToken ct)
    {
        var specs = new List<ValueTypeSpec>();

        var nominalValueAttribute = compilation.GetTypeByMetadataName($"{TypeNames.NominalValueAttributeFullName}");

        if (nominalValueAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return specs;
        }

        var emitConstructor = true;
        var maxSerializedCharLength = 0;

        foreach (var structDeclarationSyntax in structs)
        {
            ct.ThrowIfCancellationRequested();

            var semanticModel = compilation.GetSemanticModel(structDeclarationSyntax.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(structDeclarationSyntax, cancellationToken: ct) is not INamedTypeSymbol namedTypeSymbol)
            {
                continue;
            }

            // Which attribute applied?

            var valueTypeAttributeSymbols = new List<INamedTypeSymbol>();

            foreach (var attributeListSyntax in structDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    ct.ThrowIfCancellationRequested();

                    if (semanticModel.GetSymbolInfo(attributeSyntax, ct).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        continue;
                    }

                    var attributeContainingTypeSymbol = attributeSymbol.ContainingType;

                    var fullName = attributeContainingTypeSymbol.ToDisplayString();

                    if (fullName is TypeNames.NominalValueAttributeFullName
                        or TypeNames.IntervalValueAttributeFullName
                        or TypeNames.OrdinalValueAttributeFullName
                        or TypeNames.RatioValueAttributeFullName)
                    {
                        valueTypeAttributeSymbols.Add(attributeContainingTypeSymbol);

                        var attributeArgs = attributeSyntax.ArgumentList;

                        if (attributeArgs is not null)
                        {
                            var nameEquals = attributeArgs.Arguments.Where(a => a.NameEquals is not null).ToArray();

                            if (nameEquals.Any())
                            {
                                var maxSerializedCharLengthSyntax = nameEquals
                                    .Where(a => a.NameEquals is not null
                                        && a.NameEquals.Name.Identifier.ValueText == "MaxSerializedCharLength")
                                    .FirstOrDefault();

                                if (maxSerializedCharLengthSyntax is not null)
                                {
                                    var maxSerializedCharLengthOpt = semanticModel.GetConstantValue(maxSerializedCharLengthSyntax.Expression, ct);

                                    if (maxSerializedCharLengthOpt.HasValue && maxSerializedCharLengthOpt.Value is not null)
                                    {
                                        var maxSerializedCharLengthValue = (int)maxSerializedCharLengthOpt.Value;

                                        if (maxSerializedCharLengthValue > 0)
                                        {
                                            maxSerializedCharLength = maxSerializedCharLengthValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            INamedTypeSymbol? valueTypeAttributeSymbol = null;

            if (valueTypeAttributeSymbols.Count == 0)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }
            else if (valueTypeAttributeSymbols.Count == 2)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            valueTypeAttributeSymbol = valueTypeAttributeSymbols[0];

            var valueTypeFullName = valueTypeAttributeSymbol.ToDisplayString();

            var valueTypeKind = valueTypeFullName switch
            {
                TypeNames.NominalValueAttributeFullName => ValueTypeKind.Nominal,
                TypeNames.IntervalValueAttributeFullName => ValueTypeKind.Interval,
                TypeNames.OrdinalValueAttributeFullName => ValueTypeKind.Ordinal,
                TypeNames.RatioValueAttributeFullName => ValueTypeKind.Ratio,
                _ => throw new InvalidOperationException()
            };

            ct.ThrowIfCancellationRequested();

            var typeName = namedTypeSymbol.Name;

            var ns = GetNamespace(structDeclarationSyntax);

            var parent = GetParentClasses(structDeclarationSyntax);

            var implementedInterfaces = namedTypeSymbol.Interfaces;

            var valueTypeInterface = implementedInterfaces.SingleOrDefault(IsValueTypeInterface);

            ct.ThrowIfCancellationRequested();

            if (valueTypeInterface is null)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            string? containedTypeName = null;

            if (valueTypeInterface.TypeArguments.Length == 2)
            {
                containedTypeName = valueTypeInterface.TypeArguments[1].Name;
            }
            else
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            // what members are defined?

            var members = structDeclarationSyntax.Members;

            // constructor?

            var constructorsWithTwoParams = members
                .OfType<ConstructorDeclarationSyntax>()
                .Where(s => s.ParameterList.Parameters.Count == 2
                    && s.ParameterList.Parameters[0] is ParameterSyntax paramSyntax
                    && paramSyntax.Type is PredefinedTypeSyntax preDefSyntax)
                .Where(s => semanticModel.GetDeclaredSymbol(s, ct) is IMethodSymbol methodSymbol)
                .SingleOrDefault();

            if (constructorsWithTwoParams is not null)
            {
                var param1TypeSymbol = semanticModel.GetDeclaredSymbol(
                    constructorsWithTwoParams.ParameterList.Parameters[0], cancellationToken: ct);

                var paramSymbolTypeName = param1TypeSymbol!.Type.Name;

                if (paramSymbolTypeName == containedTypeName)
                {
                    var param2TypeSymbol = semanticModel.GetDeclaredSymbol(
                        constructorsWithTwoParams.ParameterList.Parameters[1], cancellationToken: ct);

                    var param2SymbolTypeName = param2TypeSymbol!.Type.Name;

                    if (param2SymbolTypeName == "Boolean")
                    {
                        // constructor defined
                        emitConstructor = false;
                    }
                }
            }

            var methods = members.OfType<MethodDeclarationSyntax>().ToArray();
            var staticMethods = methods.Where(s => s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();
            var instanceMethods = methods.Where(s => !s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

            var properties = members.OfType<PropertyDeclarationSyntax>().ToArray();
            var staticProperties = properties.Where(s => s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();
            var instanceProperties = properties.Where(s => !s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

            var fields = members.OfType<FieldDeclarationSyntax>().ToArray();
            var staticFields = fields.Where(s => s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

            // Default value field?

            var defaultValueField = staticFields.FirstOrDefault(s => s.Declaration.Variables.Any(v => v.Identifier.ValueText == "s_defaultValue"));

            var getStringMethods = staticMethods.Where(s => s.Identifier.ValueText == "GetString").ToArray();

            var useGetStringMethod = false;
            var useGetStringSpanMethod = false;

            if (getStringMethods.Length > 0)
            {
                // GetString(string)

                useGetStringMethod = getStringMethods.Any(s => s.ParameterList.Parameters.Count == 1
                    && s.ParameterList.Parameters[0] is ParameterSyntax ps
                    && ps.Type is PredefinedTypeSyntax pds
                    && pds.Keyword.Text == "string");

                // GetString(ReadOnlySpan<char>)

                useGetStringSpanMethod = getStringMethods.Any(s => s.ParameterList.Parameters.Count == 1
                    && s.ParameterList.Parameters[0] is ParameterSyntax ps
                    && ps.Type is GenericNameSyntax gns
                    && gns.Identifier.ValueText == "ReadOnlySpan"
                    && gns.TypeArgumentList.Arguments.Count == 1
                    && gns.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pds
                    && pds.Keyword.Text == "char");
            }

            var structMembers = namedTypeSymbol.GetMembers();

            var staticFieldSymbols = new List<string>(structMembers.Length);

            foreach (var member in structMembers)
            {
                if (member is IFieldSymbol field && field.IsStatic)
                {
                    staticFieldSymbols.Add(member.Name);
                }
            }

            ct.ThrowIfCancellationRequested();

            var spec = valueTypeKind switch
            {
                ValueTypeKind.Nominal => new NominalValueTypeSpec
                {
                },
                ValueTypeKind.Ordinal => new OrdinalValueTypeSpec
                {
                },
                ValueTypeKind.Interval => new IntervalValueTypeSpec
                {
                },
                ValueTypeKind.Ratio => new RatioValueTypeGenerationSpec
                {
                },
                _ => throw new NotImplementedException()
            };

            spec.ValueTypeName = typeName;
            spec.ContainedValueTypeName = containedTypeName;
            spec.Namespace = ns;
            spec.Accessibility = namedTypeSymbol.DeclaredAccessibility;
            spec.ParentClass = parent;
            spec.Fields = staticFieldSymbols;
            spec.EmitConstructor = emitConstructor;

            spec.UseDefaultValueField = defaultValueField is not null;
            spec.UseGetString = useGetStringMethod;
            spec.UseGetStringSpan = useGetStringSpanMethod;

            if (maxSerializedCharLength > 0)
            {
                spec.MaxSerializedCharLength = maxSerializedCharLength;
            }

            specs.Add(spec);
        }

        return specs;
    }

    private static bool IsValueTypeInterface(INamedTypeSymbol? interfaceSymbol)
    {
        return interfaceSymbol is not null
            && (interfaceSymbol.Name == "INominalValue"
                || interfaceSymbol.Name == "IOrdinalValue"
                || interfaceSymbol.Name == "IIntervalValue"
                || interfaceSymbol.Name == "IRatioValue");
    }

    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        var nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration (could be a nested type, for example)
        var potentialNamespaceParent = syntax.Parent;

        // Keep moving "out" of nested classes etc until we get to a namespace or until we run out of parents
        while (potentialNamespaceParent is not null and
                not NamespaceDeclarationSyntax
                and not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        return nameSpace;
    }

    private static ParentClass? GetParentClasses(BaseTypeDeclarationSyntax typeSyntax)
    {
        // Try and get the parent syntax. If it isn't a type like class/struct, this will be null
        var parentSyntax = typeSyntax.Parent as TypeDeclarationSyntax;
        ParentClass? parentClassInfo = null;

        // Keep looping while we're in a supported nested type
        while (parentSyntax != null && IsAllowedKind(parentSyntax.Kind()))
        {
            // Record the parent type keyword (class/struct etc), name, and constraints
            parentClassInfo = new ParentClass(
                keyword: parentSyntax.Keyword.ValueText,
                name: parentSyntax.Identifier.ToString() + parentSyntax.TypeParameterList,
                constraints: parentSyntax.ConstraintClauses.ToString(),
                child: parentClassInfo); // set the child link (null initially)

            // Move to the next outer type
            parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
        }

        // return a link to the outermost parent type
        return parentClassInfo;

    }

    // We can only be nested in class/struct/record
    private static bool IsAllowedKind(SyntaxKind kind) =>
        kind is SyntaxKind.ClassDeclaration or
        SyntaxKind.StructDeclaration or
        SyntaxKind.RecordDeclaration;
}
