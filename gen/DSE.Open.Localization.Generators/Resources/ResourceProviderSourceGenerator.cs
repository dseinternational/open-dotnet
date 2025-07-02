// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Diagnostics;
using DSE.Open.Localization.Generators.Resources.Parsing;
using Microsoft.CodeAnalysis;

namespace DSE.Open.Localization.Generators.Resources;

[Generator]
public sealed class ResourceProviderSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // _ = System.Diagnostics.Debugger.Launch();

        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "DSE.Open.Localization.Resources.ResourceProviderAttribute",
            static (_, _) => true,
            SyntaxProviderTransform
        );

        var textsProvider = context.AdditionalTextsProvider.Where(x => x.Path.EndsWith(".restext"));

        var compilation = context.CompilationProvider
            .Combine(provider.Collect())
            .Combine(textsProvider.Collect());

        context.RegisterSourceOutput(compilation, Execute);
    }

    private static List<ResourceProviderInformation> SyntaxProviderTransform(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        List<ResourceProviderInformation> models = [];

        var provider = (INamedTypeSymbol)context.TargetSymbol;
        var providerName = provider.Name;

        foreach (var attr in context.Attributes)
        {
            if (attr.ConstructorArguments[0].Value is not INamedTypeSymbol typeSymbol)
            {
                continue;
            }

            var stringsType = typeSymbol.OriginalDefinition;

            // `stringsType` is defined in the same folder as the resource files.
            // Get the path to the `stringsType` file.
            var path = (stringsType.Locations[0].SourceTree?.FilePath)
                       ?? throw new InvalidOperationException("Resource file path not found.");

            var accessibility = provider.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                Accessibility.Private => "private",
                Accessibility.Protected => "protected",
                Accessibility.ProtectedAndInternal => "protected internal",
                Accessibility.ProtectedOrInternal => "protected internal",
                Accessibility.NotApplicable => throw new InvalidOperationException("Invalid accessibility."),
                _ => throw new InvalidOperationException("Invalid accessibility.")
            };

            var model = new ResourceProviderInformation
            {
                ProviderName = providerName,
                ProviderNamespace = provider.ContainingNamespace.ToDisplayString(),
                ProviderAccessibility = accessibility,
                ResourcesName =
                    stringsType.OriginalDefinition.ToDisplayString(DisplayFormats
                        .NameOnly),
                ResourcesFullyQualifiedName =
                    stringsType.OriginalDefinition.ToDisplayString(DisplayFormats
                        .FullyQualifiedNonGenericWithGlobalPrefix),
                ResourcesPath = path
            };

            models.Add(model);
        }

        return models;
    }

    private static void Execute(
        SourceProductionContext context,
        ((Compilation Left, ImmutableArray<List<ResourceProviderInformation>> Right) Left,
            ImmutableArray<AdditionalText> Right) tuple)
    {
        var ((_, modelsList), additionalTexts) = tuple;
        var models = modelsList.SelectMany(x => x);

        Execute(context, models, additionalTexts, context.CancellationToken);
    }

    private static void Execute(
        SourceProductionContext context,
        IEnumerable<ResourceProviderInformation> models,
        ImmutableArray<AdditionalText> additionalTexts,
        CancellationToken cancellationToken = default)
    {
        foreach (var model in models)
        {
            var resourceItems = GetResourceItems(model, additionalTexts, context.ReportDiagnostic,
                cancellationToken);
            var source = ResourceProviderEmitter.Emit(model, resourceItems);
            context.AddSource($"{model.ProviderName}.g.cs", source);
        }
    }

    private static HashSet<ResourceItem> GetResourceItems(
        ResourceProviderInformation model,
        ImmutableArray<AdditionalText> additionalTexts,
        Action<Diagnostic> reportDiagnostic,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetFileForResource(model, additionalTexts, out var nullableFile))
        {
            reportDiagnostic(Diagnostics.ResourceFileNotFound(model.ResourcesPath));
            return [];
        }

        // [NotNullWhen(true)]
        var file = nullableFile!;

        var content = file.GetText(cancellationToken);

        if (content is null)
        {
            reportDiagnostic(Diagnostics.FailedToReadFile(file.Path));
            return [];
        }

        HashSet<ResourceItem> keys = [];

        foreach (var line in content.Lines)
        {
            if (!ResourceValueParser.TryParseLine(line, file.Path, reportDiagnostic, out var resourceItem))
            {
                continue;
            }

            if (!keys.Add(resourceItem!))
            {
                reportDiagnostic(Diagnostics.DuplicateKey(file.Path, line, resourceItem!.Key));
            }
        }

        return keys;
    }

    private static bool TryGetFileForResource(
        ResourceProviderInformation model,
        ImmutableArray<AdditionalText> files,
        out AdditionalText? file)
    {
        var dir = Path.GetDirectoryName(model.ResourcesPath)
                  ?? throw new InvalidOperationException(
                      $"Could not get directory name for path '{model.ResourcesPath}'.");

        file = null;

        foreach (var text in files)
        {
            var textDir = Path.GetDirectoryName(text.Path)
                          ?? throw new InvalidOperationException(
                              $"Could not get directory name for path '{text.Path}'.");

            var fileName = Path.GetFileNameWithoutExtension(text.Path);

            if (textDir == dir && model.ResourcesName == fileName)
            {
                file = text;
                break;
            }
        }

        return file is not null;
    }
}
