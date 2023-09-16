// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using DSE.Open.Values.Generators.Extensions;
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
            .Where(static m => m is not null);

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

                if (fullName is TypeNames.EquatableValueAttributeFullName
                    or TypeNames.AddableValueAttributeFullName
                    or TypeNames.ComparableValueAttributeFullName
                    or TypeNames.DivisibleValueAttributeFullName)
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

        var nominalValueAttribute = compilation.GetTypeByMetadataName($"{TypeNames.EquatableValueAttributeFullName}");

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

                    if (fullName is not (TypeNames.EquatableValueAttributeFullName
                        or TypeNames.AddableValueAttributeFullName
                        or TypeNames.ComparableValueAttributeFullName
                        or TypeNames.DivisibleValueAttributeFullName))
                    {
                        continue;
                    }

                    valueTypeAttributeSymbols.Add(attributeContainingTypeSymbol);

                    var attributeArgs = attributeSyntax.ArgumentList;

                    if (attributeArgs is null)
                    {
                        continue;
                    }

                    var nameEquals = attributeArgs.Arguments.Where(a => a.NameEquals is not null).ToArray();

                    if (nameEquals.Length == 0)
                    {
                        continue;
                    }

                    var maxSerializedCharLengthSyntax = nameEquals.FirstOrDefault(a =>
                        a.NameEquals is not null
                        && a.NameEquals.Name.Identifier.ValueText == "MaxSerializedCharLength");

                    if (maxSerializedCharLengthSyntax is not null)
                    {
                        var maxSerializedCharLengthOpt = semanticModel.GetConstantValue(maxSerializedCharLengthSyntax.Expression, ct);

                        if (maxSerializedCharLengthOpt is { HasValue: true, Value: not null })
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

            if (valueTypeAttributeSymbols.Count == 0)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            if (valueTypeAttributeSymbols.Count == 2)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            var valueTypeAttributeSymbol = valueTypeAttributeSymbols[0];

            var valueTypeFullName = valueTypeAttributeSymbol.ToDisplayString();

            var valueTypeKind = valueTypeFullName switch
            {
                TypeNames.EquatableValueAttributeFullName => ValueTypeKind.Equatable,
                TypeNames.AddableValueAttributeFullName => ValueTypeKind.Addable,
                TypeNames.ComparableValueAttributeFullName => ValueTypeKind.Comparable,
                TypeNames.DivisibleValueAttributeFullName => ValueTypeKind.Divisible,
                _ => throw new InvalidOperationException()
            };

            ct.ThrowIfCancellationRequested();

            var ns = GetNamespace(structDeclarationSyntax);

            var parent = GetParentClasses(structDeclarationSyntax);

            var implementedInterfaces = namedTypeSymbol.Interfaces;

            var valueTypeInterface = implementedInterfaces.SingleOrDefault(IsValueTypeInterface);

            var utf8SerializableInterface = implementedInterfaces.SingleOrDefault(i => i.Name == TargetNames.IUtf8SpanSerializableInterfaceName);

            ct.ThrowIfCancellationRequested();

            if (valueTypeInterface is null)
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            string? containedTypeName;

            if (valueTypeInterface.TypeArguments.Length == 2)
            {
                containedTypeName = valueTypeInterface.TypeArguments[1].Name;
            }
            else
            {
                // TODO: diagnostics
                throw new InvalidOperationException();
            }

            //if (containedTypeName == "ClinicalConceptCode")
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            // what interfaces does the contained type implement?
            var containedTypeSymbol = valueTypeInterface.TypeArguments[1];

            var containedTypeInterfaces = containedTypeSymbol.Interfaces;

            var emitTryParseUtf8SpanMethod = false;
            var emitParseUtf8Method = false;
            var emitTryFormatUtf8Method = false;
            var emitUtf8SpanSerializableInterface = false;

            // Only attempt to find UTF8 interfaces on contained type if `IUtf8SpanSerializable` interface is present
            if (utf8SerializableInterface is not null)
            {
                foreach (var i in containedTypeInterfaces)
                {
                    if (i.Name == TargetNames.IUtf8SpanSerializableInterfaceName)
                    {
                        emitTryFormatUtf8Method = true;
                        emitTryParseUtf8SpanMethod = true;
                        emitParseUtf8Method = true;
                        emitUtf8SpanSerializableInterface = true;
                        break;
                    }

                    if (i.Name == TargetNames.IUtf8SpanFormattableInterfaceName)
                    {
                        emitTryFormatUtf8Method = true;
                        continue;
                    }

                    if (i.Name == TargetNames.IUtf8SpanParsableInterfaceName)
                    {
                        emitTryParseUtf8SpanMethod = true;
                        emitParseUtf8Method = true;
                        continue;
                    }
                }

                if (!emitUtf8SpanSerializableInterface)
                {
                    // If both `IUtf8SpanFormattable` and `IUtf8SpanParsable` are implemented, then we can emit the `IUtf8SpanSerializable` interface
                    emitUtf8SpanSerializableInterface = emitTryFormatUtf8Method && emitTryParseUtf8SpanMethod && emitParseUtf8Method;
                }
            }

            // what members are defined?

            var members = structDeclarationSyntax.Members;

            // constructor?

            var constructorsWithTwoParams = members
                .OfType<ConstructorDeclarationSyntax>()
                .Where(s =>
                    s.ParameterList.Parameters.Count == 2
                    && s.ParameterList.Parameters[0] is { } paramSyntax
                    && paramSyntax.Type is PredefinedTypeSyntax)
                .SingleOrDefault(s => semanticModel.GetDeclaredSymbol(s, ct) != null);

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

            // var properties = members.OfType<PropertyDeclarationSyntax>().ToArray();
            // var staticProperties = properties.Where(s => s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();
            // var instanceProperties = properties.Where(s => !s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

            // var fields = members.OfType<FieldDeclarationSyntax>().ToArray();
            // var staticFields = fields.Where(s => s.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

            var useGetStringMethod = false;
            var useGetStringSpanMethod = false;

            foreach (var method in staticMethods)
            {
                if (!useGetStringMethod && method.Identifier.ValueText == TargetNames.GetStringMethodName)
                {
                    {
                        useGetStringMethod = method.ParameterList.Parameters.Count == 1
                            && method.ParameterList.Parameters[0] is ParameterSyntax ps
                            && ps.Type is PredefinedTypeSyntax pds
                            && pds.Keyword.Text == "string";
                    }

                    {
                        useGetStringSpanMethod = method.ParameterList.Parameters.Count == 1
                            && method.ParameterList.Parameters[0] is ParameterSyntax ps
                            && ps.Type is GenericNameSyntax gns
                            && gns.Identifier.ValueText == "ReadOnlySpan"
                            && gns.TypeArgumentList.Arguments.Count == 1
                            && gns.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pds
                            && pds.Keyword.Text == "char";
                    }
                }

                // IUtf8SpanParsable.TryParse

                if (emitTryParseUtf8SpanMethod && method.Identifier.ValueText == TargetNames.TryParseMethodName)
                {
                    emitTryParseUtf8SpanMethod = !method.IsIUtf8SpanParseableTryParseMethod();
                    continue;
                }

                // IUtf8SpanParsable.Parse

                if (emitParseUtf8Method && method.Identifier.ValueText == TargetNames.ParseMethodName)
                {
                    emitParseUtf8Method = !method.IsIUtf8SpanParsableParseMethod();
                    continue;
                }
            }

            var emitEqualsMethod = true;
            var emitCompareToMethod = true;
            var emitGetHashCodeMethod = true;
            var emitTryFormatMethod = true;
            var emitToStringOverrideMethod = true;

            foreach (var method in instanceMethods)
            {
                // Equals
                if (emitEqualsMethod && method.Identifier.ValueText == TargetNames.EqualsMethodName)
                {
                    emitEqualsMethod = !(
                        method.ParameterList.Parameters.Count == 1
                        && method.ParameterList.Parameters[0] is ParameterSyntax ps
                        && ps.Type is IdentifierNameSyntax ins
                        && ins.Identifier.Text == namedTypeSymbol.Name);

                    continue;
                }

                // CompareTo
                if (emitCompareToMethod && method.Identifier.ValueText == TargetNames.CompareToMethodName)
                {
                    emitCompareToMethod = !(
                        method.ParameterList.Parameters.Count == 1
                        && method.ParameterList.Parameters[0] is ParameterSyntax ps
                        && ps.Type is IdentifierNameSyntax ins
                        && ins.Identifier.Text == namedTypeSymbol.Name);

                    continue;
                }

                if (emitGetHashCodeMethod && method.Identifier.ValueText == TargetNames.GetHashCodeMethodName)
                {
                    emitGetHashCodeMethod = method.ParameterList.Parameters.Count != 0;
                    continue;
                }

                if (method.Identifier.ValueText == TargetNames.TryFormatMethodName)
                {
                    if (emitTryFormatMethod)
                    {
                        emitTryFormatMethod = !method.IsISpanFormattableTryFormatMethod();

                        if (!emitTryFormatMethod)
                        {
                            continue;
                        }
                    }

                    if (emitTryFormatUtf8Method)
                    {
                        emitTryFormatUtf8Method = !method.IsIUtf8SpanFormattableTryFormatMethod();

                        if (!emitTryFormatUtf8Method)
                        {
                            continue;
                        }
                    }

                    continue;
                }

                // ToString

                if (emitToStringOverrideMethod && method.Identifier.ValueText == TargetNames.ToStringMethodName)
                {
                    emitToStringOverrideMethod = method.ParameterList.Parameters.Count != 0;
                    continue;
                }
            }


            var structMembers = namedTypeSymbol.GetMembers();

            var staticFieldSymbols = new List<string>(structMembers.Length);

            foreach (var member in structMembers)
            {
                if (member is IFieldSymbol { IsStatic: true })
                {
                    staticFieldSymbols.Add(member.Name);
                }
            }

            ct.ThrowIfCancellationRequested();

            var spec = valueTypeKind switch
            {
                ValueTypeKind.Equatable => new EquatableValueTypeSpec
                {
                },
                ValueTypeKind.Comparable => new ComparableValueTypeSpec
                {
                    EmitCompareToMethod = emitCompareToMethod
                },
                ValueTypeKind.Addable => new AddableValueTypeSpec
                {
                    EmitCompareToMethod = emitCompareToMethod
                },
                ValueTypeKind.Divisible => new DivisibleValueTypeGenerationSpec
                {
                    EmitCompareToMethod = emitCompareToMethod
                },
                _ => throw new NotImplementedException()
            };

            spec.ValueTypeName = namedTypeSymbol.Name;
            spec.ContainedValueTypeName = containedTypeName;
            spec.Namespace = ns;
            spec.Accessibility = namedTypeSymbol.DeclaredAccessibility;
            spec.ParentClass = parent;
            spec.Fields = staticFieldSymbols;
            spec.EmitConstructor = emitConstructor;
            spec.EmitEqualsMethod = emitEqualsMethod;
            spec.EmitGetHashCodeMethod = emitGetHashCodeMethod;
            spec.EmitTryFormatMethod = emitTryFormatMethod;
            spec.EmitToStringOverrideMethod = emitToStringOverrideMethod;

            spec.UseGetString = useGetStringMethod;
            spec.UseGetStringSpan = useGetStringSpanMethod;

            if (maxSerializedCharLength > 0)
            {
                spec.MaxSerializedCharLength = maxSerializedCharLength;
            }

            spec.EmitUtf8SpanSerializableInterface = emitUtf8SpanSerializableInterface;
            spec.EmitTryFormatUtf8Method = emitTryFormatUtf8Method;
            spec.EmitParseUtf8Method = emitParseUtf8Method;
            spec.EmitTryParseUtf8Method = emitTryParseUtf8SpanMethod;

            specs.Add(spec);
        }

        return specs;
    }

    private static bool IsValueTypeInterface(INamedTypeSymbol? interfaceSymbol) =>
        interfaceSymbol?.Name is "IEquatableValue" or "IComparableValue" or "IAddableValue" or "IDivisibleValue";

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
        if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent)
        {
            return nameSpace;
        }

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
