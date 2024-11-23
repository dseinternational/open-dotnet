// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DSE.Open.EntityFrameworkCore.Models.Language.ValueGeneration;

/// <summary>
/// Generates <see cref="WordId"/> values safe for use with <c>bigint</c> (<see cref="long"/>) columns.
/// </summary>
public sealed class TemporaryWordIdValueGenerator : ValueGenerator<WordId>
{
    private ulong _next = LanguageIds.MaxIdValue;

    public override bool GeneratesTemporaryValues => true;

    public override WordId Next([NotNull] EntityEntry entry)
    {
        return new(Interlocked.Decrement(ref _next));
    }
}
