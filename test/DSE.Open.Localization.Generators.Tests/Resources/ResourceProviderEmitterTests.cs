// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Localization.Generators.Resources;
using DSE.Open.Localization.Generators.Resources.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DSE.Open.Localization.Generators.Tests.Resources;

public sealed class ResourceProviderEmitterTests
{
    [Fact]
    public Task Emit_WithWrappedSingleObjectHole()
    {
        const string key = "Key";
        const string holeText = "{object:object}";
        const string value = $"Leading text {holeText} trailing text";
        const string line = $"{key} = {value}";
        List<Diagnostic> diagnostics = [];

        var text = SourceText.From(line).Lines[0];

        var result = ResourceValueParser.TryParseLine(text, "path", diagnostics.Add, out var item);

        Assert.True(result);
        Assert.NotNull(item);
        Assert.Empty(diagnostics);

        var model = new ResourceProviderInformation
        {
            ProviderName = "ResourceProvider",
            ResourcesFullyQualifiedName = "Strings",
            ProviderNamespace = "DSE.Open.Localization.Generators.Tests.Functional",
            ProviderAccessibility = "public"
        };

        var source = ResourceProviderEmitter.Emit(model, [item]);

        return Verify(source);
    }

    [Fact]
    public Task Emit_WithConstantText()
    {
        const string line = "Key = Value";
        List<Diagnostic> diagnostics = [];

        var text = SourceText.From(line).Lines[0];

        var result = ResourceValueParser.TryParseLine(text, "path", diagnostics.Add, out var item);

        Assert.True(result);
        Assert.NotNull(item);
        Assert.Empty(diagnostics);

        var model = new ResourceProviderInformation
        {
            ProviderName = "ResourceProvider",
            ResourcesFullyQualifiedName = "Strings",
            ProviderNamespace = "DSE.Open.Localization.Generators.Tests.Functional",
            ProviderAccessibility = "public"
        };

        var source = ResourceProviderEmitter.Emit(model, [item]);

        return Verify(source);
    }

    [Fact]
    public Task Emit_WithTwoHolesOfSameKey_Untyped()
    {
        const string line = "Key = Something {0} something {1} something {0} something {1} again";
        List<Diagnostic> diagnostics = [];
        var text = SourceText.From(line).Lines[0];
        var result = ResourceValueParser.TryParseLine(text, "path", diagnostics.Add, out var item);

        Assert.True(result);
        Assert.NotNull(item);
        Assert.Empty(diagnostics);

        var model = new ResourceProviderInformation
        {
            ProviderName = "ResourceProvider",
            ResourcesFullyQualifiedName = "Strings",
            ProviderNamespace = "DSE.Open.Localization.Generators.Tests.Functional",
            ProviderAccessibility = "public"
        };

        var source = ResourceProviderEmitter.Emit(model, [item]);

        return Verify(source);
    }

    private static Task Verify(string source)
    {
        return Verifier.Verify(source).UseDirectory("VerifyOutput");
    }
}
