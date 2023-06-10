// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Values.Generators.Tests;

public class RatioValueTypeGenerationTests : ValueTypeGenerationTests
{
    public RatioValueTypeGenerationTests(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public void GeneratesValueType()
    {
        var inputCompilation = CompilationHelper.CreateCompilation(@"
using DSE.Open;
using DSE.Open.Values;

namespace TestNamespace;

#nullable enable

[RatioValue]
public readonly partial struct Percentage : IRatioValue<Percentage, int>
{
    public static Percentage Zero { get; } = new(0);

    static int ISpanSerializable<Percentage>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(int value) => value is >= 0 and <= 100;
}

[RatioValue]
public readonly partial struct PercentageF : IRatioValue<PercentageF, float>
{
    public static PercentageF Zero { get; } = new(0);

    static int ISpanSerializable<PercentageF>.MaxSerializedCharLength { get; } = 1;

    public static bool IsValidValue(float value) => value is >= 0 and <= 100;
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
}

