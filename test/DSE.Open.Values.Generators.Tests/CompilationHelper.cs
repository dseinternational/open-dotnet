// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DSE.Open.Values.Generators.Tests;

public static class CompilationHelper
{
    private static readonly CSharpParseOptions s_parseOptions = new(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse);

    private static readonly Assembly s_systemRuntimeAssembly = Assembly.Load("System.Runtime");

    public static Compilation CreateCompilation(

        string source,
        MetadataReference[]? additionalReferences = null,
        string assemblyName = "TestAssembly")
    {
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(s_systemRuntimeAssembly.Location),

            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),

            // System.ObjectModel
            MetadataReference.CreateFromFile(typeof(TypeConverterAttribute).GetTypeInfo().Assembly.Location),

            // DSE.Open.Abstractions
            MetadataReference.CreateFromFile(typeof(ITryConvertibleFrom<,>).GetTypeInfo().Assembly.Location),

            // DSE.Open.ValueTypes.Abstractions
            MetadataReference.CreateFromFile(typeof(RatioValueAttribute).GetTypeInfo().Assembly.Location),

            // DSE.Open.ValueTypes
            MetadataReference.CreateFromFile(typeof(ValueFormatter).GetTypeInfo().Assembly.Location),
        };

        if (additionalReferences is not null)
        {
            foreach (var r in additionalReferences)
            {
                references.Add(r);
            }
        }

        return CSharpCompilation.Create(
            assemblyName,
            new[] { CSharpSyntaxTree.ParseText(source) },
            references.ToArray(),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    public static CSharpGeneratorDriver CreateValuesSourceGeneratorDriver(ValueTypesGenerator? generator = null)
    {
        generator ??= new();

        return CSharpGeneratorDriver.Create(
            generators: new ISourceGenerator[] { generator.AsSourceGenerator() },
            parseOptions: s_parseOptions,
            driverOptions: new GeneratorDriverOptions(
                disabledOutputs: IncrementalGeneratorOutputKind.None,
                trackIncrementalGeneratorSteps: true)
            );
    }

    public static SourceGenerationResult RunValuesSourceGenerator(Compilation compilation)
    {
        var driver = CreateValuesSourceGeneratorDriver();

        _ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outCompilation, out var diagnostics);

        return new()
        {
            NewCompilation = outCompilation,
            Diagnostics = diagnostics,
        };
    }
}
