// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace DSE.Open.Values.Generators.Tests;

public abstract class ValueTypeGenerationTests
{
    protected ValueTypeGenerationTests(ITestOutputHelper testOutput)
    {
        TestOutput = testOutput;
    }

    public ITestOutputHelper TestOutput { get; }

    public void AssertDiagnosticsCount(int expectedCount, ImmutableArray<Diagnostic> diagnostics)
    {
        if (diagnostics.Length != expectedCount)
        {
            TestOutput.WriteLine("Unexpected diagnostics:");

            foreach (var diagnostic in diagnostics)
            {
                TestOutput.WriteLine(diagnostic.ToString());
            }
        }

        Assert.Equal(expectedCount, diagnostics.Length);
    }

    public void WriteSyntax(SyntaxTree syntaxTree)
    {
        Assert.NotNull(syntaxTree);
        TestOutput.WriteLine("----------------------------------------------------------");
        TestOutput.WriteLine(syntaxTree?.ToString());
        TestOutput.WriteLine("----------------------------------------------------------");
    }
}

