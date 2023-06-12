// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Values.Generators.Tests;

public class DivisibleValueTypeGenerationTests : ValueTypeGenerationTests
{
    public DivisibleValueTypeGenerationTests(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public void GeneratesValueType()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[DivisibleValue]
public readonly partial struct Percentage : IDivisibleValue<Percentage, int>
{
    public static Percentage Zero { get; } = new(0);

    public static int MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(int value) => value is >= 0 and <= 100;
}

[DivisibleValue]
public readonly partial struct PercentageF : IDivisibleValue<PercentageF, float>
{
    public static PercentageF Zero { get; } = new(0);

    public static int MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(float value) => value is >= 0 and <= 100;
}

#nullable disable
");

        var result = CompilationHelper.RunValuesSourceGenerator(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(3, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }
}

