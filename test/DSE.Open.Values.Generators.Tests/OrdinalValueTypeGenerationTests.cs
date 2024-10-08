// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Values.Generators.Tests;

public class ComparableValueTypeGenerationTests : ValueTypeGenerationTests
{
    public ComparableValueTypeGenerationTests(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public void GeneratesValueType()
    {
        var inputCompilation = CompilationHelper.CreateCompilation("""

                                                                   using DSE.Open.Values;

                                                                   namespace TestNamespace;

                                                                   #nullable enable

                                                                   [ComparableValue]
                                                                   public readonly partial struct MyOptions : IComparableValue<MyOptions, byte>
                                                                   {
                                                                       public static readonly MyOptions Option1;
                                                                       public static readonly MyOptions Option2 = new(1);

                                                                       public static int MaxSerializedCharLength { get; } = 1;

                                                                       public static bool IsValidValue(byte value) => value is >= 0 and <= 1;
                                                                   }

                                                                   #nullable disable

                                                                   """);

        var result = CompilationHelper.RunValuesSourceGenerator(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics(TestContext.Current.CancellationToken);

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }
}

