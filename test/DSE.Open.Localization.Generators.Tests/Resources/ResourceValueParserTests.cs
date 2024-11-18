// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Localization.Generators.Resources;
using DSE.Open.Localization.Generators.Resources.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DSE.Open.Localization.Generators.Tests.Resources;

#pragma warning disable CA1307 // Specify StringComparison for clarity

public sealed class ResourceValueParserTests
{
    [Fact]
    public void TryParseValue_WithSingleHoleWithIntegerIndex()
    {
        // Arrange
        const string value = "Hello {0}!";
        var text = SourceText.From(value).Lines[0];

        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.Empty(diagnostics);

        var actual = Assert.Single(holes);
        Assert.Equal(value.IndexOf('{'), actual.Index);
        Assert.Equal(3, actual.Length);
        Assert.Equal("arg0", actual.Name);
        Assert.Null(actual.Type);
    }

    [Fact]
    public void TryParseValue_WithSingleNamedHole()
    {
        // Arrange
        const string name = "World";
        const string value = $$"""Hello {{{name}}}!""";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.True(result);
        Assert.Empty(diagnostics);

        var actual = Assert.Single(holes);
        Assert.Equal(value.IndexOf('{'), actual.Index);
        Assert.Equal(7, actual.Length);
        Assert.Equal(name.ToLowerInvariant(), actual.Name);
        Assert.Null(actual.Type);
    }

    [Fact]
    public void TryParseValue_WithSingleNamedAndTypedHole()
    {
        // Arrange
        const string name = "World";
        const string type = "string";
        const string value = $$"""Hello {{{name}}:{{type}}}!""";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.True(result);
        Assert.Empty(diagnostics);
        var actual = Assert.Single(holes);
        Assert.Equal(value.IndexOf('{'), actual.Index);
        Assert.Equal(name.Length + type.Length + 3, actual.Length);
        Assert.Equal(name.ToLowerInvariant(), actual.Name);
        Assert.Equal(type, actual.Type);
    }

    [Fact]
    public void TryParseValue_WithTwoIndexedAndUntypedHoles()
    {
        // Arrange
        const string value = "Hello {0} and {1}!";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.True(result);
        Assert.Empty(diagnostics);
        Assert.Equal(2, holes.Count);

        var first = holes[0];
        var second = holes[1];

        Assert.Equal(value.IndexOf('{'), first.Index);
        Assert.Equal(3, first.Length);
        Assert.Equal("arg0", first.Name);
        Assert.Null(first.Type);

        Assert.Equal(value.LastIndexOf('{'), second.Index);
        Assert.Equal(3, second.Length);
        Assert.Equal("arg1", second.Name);
        Assert.Null(second.Type);
    }

    [Fact]
    public void TryParseValue_WithOneIndexedAndOneTypedHoles()
    {
        // Arrange
        const string value = "Hello {0} and {1:string}!";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.True(result);
        Assert.Empty(diagnostics);

        Assert.Equal(2, holes.Count);
        var first = holes[0];
        var second = holes[1];

        Assert.Equal(value.IndexOf('{'), first.Index);
        Assert.Equal(3, first.Length);
        Assert.Equal("arg0", first.Name);
        Assert.Null(first.Type);

        Assert.Equal(value.LastIndexOf('{'), second.Index);
        Assert.Equal(10, second.Length);
        Assert.Equal("arg1", second.Name);
        Assert.Equal("string", second.Type);
    }

    [Fact]
    public void TryParseValue_WithTwoNamedAndTypedHoles()
    {
        // Arrange
        const string name1 = "Name";
        const string name2 = "Age";
        const string type1 = "string";
        const string type2 = "int";

        const string value = $$"""Hello {{{name1}}:{{type1}}} and {{{name2}}:{{type2}}}!""";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.True(result);
        Assert.Empty(diagnostics);
        Assert.Equal(2, holes.Count);

        var first = holes[0];
        var second = holes[1];

        Assert.Equal(value.IndexOf('{'), first.Index);
        Assert.Equal(name1.Length + type1.Length + 3, first.Length);
        Assert.Equal(name1.ToLowerInvariant(), first.Name);
        Assert.Equal(type1, first.Type);

        Assert.Equal(value.LastIndexOf('{'), second.Index);
        Assert.Equal(name2.Length + type2.Length + 3, second.Length);
        Assert.Equal(name2.ToLowerInvariant(), second.Name);
        Assert.Equal(type2, second.Type);
    }

    [Fact]
    public void TryParse_WithNameAndInvalidUntyped_ShouldReturnFalse()
    {
        // Arrange
        const string value = "An invalid {name:} with no type";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.False(result);
        Assert.Empty(holes);

        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(Diagnostics.NoTypeInHoleDiagnosticId, diagnostic.Id);
    }

    [Fact]
    public void TryParseLine_WithSingleUntypedValue()
    {
        // Arrange
        const string key = "Something";
        const string value = "t{0}t";
        const string source = $"{key} = {value}";
        var text = SourceText.From(source).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseLine(text, "path", diagnostics.Add, out var item);

        // Assert
        Assert.True(result);
        Assert.NotNull(item);
        Assert.Empty(diagnostics);

        Assert.Equal(key, item.Key);

        var hole = Assert.Single(item.Holes);
        Assert.Equal(value.IndexOf('{'), hole.Index);
        Assert.Equal(3, hole.Length);
        Assert.Equal("arg0", hole.Name);
        Assert.Null(hole.Type);
    }

    [Fact]
    public void TryParseLine_WithTwoTypedValues()
    {
        const string key = "Something";
        const string type1 = "string";
        const string type2 = "int";

        const string value = $$"""t {0:{{type1}}} and {1:{{type2}}} t""";
        const string source = $"{key} = {value}";

        var text = SourceText.From(source).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseLine(text, "path", diagnostics.Add, out var item);

        // Assert
        Assert.True(result);
        Assert.NotNull(item);
        Assert.Empty(diagnostics);
        Assert.Equal(key, item.Key);

        Assert.Equal(2, item.Holes.Count);

        var first = item.Holes[0];
        var second = item.Holes[1];

        Assert.Equal(value.IndexOf('{'), first.Index);
        Assert.Equal("0".Length + type1.Length + 3, first.Length);
        Assert.Equal("arg0", first.Name);
        Assert.Equal(type1, first.Type);

        Assert.Equal(value.LastIndexOf('{'), second.Index);
        Assert.Equal("1".Length + type2.Length + 3, second.Length);
        Assert.Equal("arg1", second.Name);
        Assert.Equal(type2, second.Type);
    }

    [Fact]
    public void TryParseValue_WithConflictingTypeConstraints_ShouldReturnError()
    {
        // Arrange
        const string value = "Hello {0:int} and {0:string}!";
        var text = SourceText.From(value).Lines[0];
        List<Diagnostic> diagnostics = [];

        // Act
        var result = ResourceValueParser.TryParseValue(value, "path", text, 0, diagnostics.Add, out var holes);

        // Assert
        Assert.False(result);
        Assert.Empty(holes);
        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(Diagnostics.ConflictingTypeConstraintDiagnosticId, diagnostic.Id);
    }
}
