// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Values.Generators.Tests;

public class NominalValueTypeGenerationTests : ValueTypeGenerationTests
{
    public NominalValueTypeGenerationTests(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public void Generates_type_given_minimal_specification()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open;
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[NominalValue]
public readonly partial struct MyOptions : INominalValue<MyOptions, byte>
{
    public static readonly MyOptions Option1;
    public static readonly MyOptions Option2 = new(1);

    static int ISpanSerializable<MyOptions>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(byte value) => value is >= 0 and <= 1;
}

[NominalValue]
internal readonly partial struct MyOptions2 : INominalValue<MyOptions2, byte>
{
#pragma warning disable CS0649
    public static readonly MyOptions2 Option1;
#pragma warning restore CS0649
    public static readonly MyOptions2 Option2 = new(1);

    static int ISpanSerializable<MyOptions2>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(byte value) => value is >= 0 and <= 1;
}

#nullable disable
");

        var result = CompilationHelper.RunValuesGenerators(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(3, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }

    [Fact]
    public void Generates_type_given_user_defined_constructor()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open;
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[NominalValue]
public readonly partial struct MyOptions : INominalValue<MyOptions, long>
{
    public static readonly MyOptions Option1;
    public static readonly MyOptions Option2 = new(1);
    public static readonly MyOptions Option3 = new(2);

    static int ISpanSerializable<MyOptions>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(long value) => value is >= 0 and <= 2;

    private MyOptions(long value)
    {
        _value = value;
    }
}

#nullable disable
");

        var result = CompilationHelper.RunValuesGenerators(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }

    [Fact]
    public void Generates_type_given_MaxSerializedCharLength()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[NominalValue(MaxSerializedCharLength = 1)]
public readonly partial struct MyOptions : INominalValue<MyOptions, long>
{
    public static readonly MyOptions Option1;
    public static readonly MyOptions Option2 = new(1);
    public static readonly MyOptions Option3 = new(2);

    public static bool IsValidValue(long value) => value is >= 0 and <= 2;

    private MyOptions(long value)
    {
        _value = value;
    }
}

#nullable disable
");

        var result = CompilationHelper.RunValuesGenerators(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }

    [Fact]
    public void Generates_type_with_custom_default_value()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open;
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[NominalValue]
public readonly partial struct MyOptions : INominalValue<MyOptions, long>
{
    public static readonly MyOptions Option1;
    public static readonly MyOptions Option2 = new(1);
    public static readonly MyOptions Option3 = new(2);

    private static readonly long s_defaultValue = 2;

    static int ISpanSerializable<MyOptions>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(long value) => value is >= 0 and <= 2;
}

#nullable disable
");

        var result = CompilationHelper.RunValuesGenerators(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }

    [Fact]
    public void Generates_type_using_getstring_methods()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using System;
using DSE.Open;
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[NominalValue]
public readonly partial struct MyOptions : INominalValue<MyOptions, long>
{
    public static readonly MyOptions Option1;
    public static readonly MyOptions Option2 = new(1);
    public static readonly MyOptions Option3 = new(2);

    private static readonly long s_defaultValue = 2;

    static int ISpanSerializable<MyOptions>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(long value) => value is >= 0 and <= 2;

    private static string GetString(string s) => string.IsInterned(s) ?? s;

    private static string GetString(ReadOnlySpan<char> s) => GetString(s.ToString());
}

#nullable disable
");
        
        var result = CompilationHelper.RunValuesGenerators(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }
}
