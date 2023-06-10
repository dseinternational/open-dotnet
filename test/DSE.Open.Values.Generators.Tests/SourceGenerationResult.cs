// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace DSE.Open.Values.Generators.Tests;

public sealed record SourceGenerationResult
{
    public required Compilation NewCompilation { get; init; }

    public ImmutableArray<Diagnostic> Diagnostics { get; init; }
}
