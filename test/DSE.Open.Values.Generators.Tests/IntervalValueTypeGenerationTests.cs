// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Values.Generators.Tests;

public class AddableValueTypeGenerationTests : ValueTypeGenerationTests
{
    public AddableValueTypeGenerationTests(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public void GeneratesValueType()
    {
        var inputCompilation = CompilationHelper.CreateCompilation("""

                                                                   using DSE.Open.Values;

                                                                   namespace TestNamespace;

                                                                   #nullable enable

                                                                   [AddableValue]
                                                                   public readonly partial struct MyOptions : IAddableValue<MyOptions, byte>
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

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }

    [Fact]
    public void Generates_type_using_user_compareto_method()
    {
        var inputCompilation = CompilationHelper.CreateCompilation("""

                                                                   using DSE.Open.Values;

                                                                   namespace TestNamespace;

                                                                   #nullable enable

                                                                   [AddableValue]
                                                                   public readonly partial struct MyOptions : IAddableValue<MyOptions, long>
                                                                   {
                                                                       public static readonly MyOptions Option1;
                                                                       public static readonly MyOptions Option2 = new(1);
                                                                       public static readonly MyOptions Option3 = new(2);
                                                                   
                                                                       public static int MaxSerializedCharLength { get; } = 1;
                                                                   
                                                                       public static bool IsValidValue(long value) => value is >= 0 and <= 2;
                                                                   
                                                                       public int CompareTo(MyOptions other) => _value.CompareTo(other._value);
                                                                   }

                                                                   #nullable disable

                                                                   """);

        var result = CompilationHelper.RunValuesSourceGenerator(inputCompilation);

        AssertDiagnosticsCount(0, result.Diagnostics);

        var outputSyntaxTrees = result.NewCompilation.SyntaxTrees.ToImmutableArray();

        Assert.Equal(2, outputSyntaxTrees.Length);

        WriteSyntax(outputSyntaxTrees[1]);

        var newCompilationDiagnostics = result.NewCompilation.GetDiagnostics();

        AssertDiagnosticsCount(0, newCompilationDiagnostics);
    }
}

