// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DSE.Open.EntityFrameworkCore.ValueGeneration;

/// <summary>
/// Generates <see cref="ulong"/> values safe for use with <c>bigint</c> (<see cref="long"/>) columns.
/// </summary>
public sealed class TemporaryUInt64ValueGenerator : ValueGenerator<ulong>
{
    private ulong _next = long.MaxValue;

    public override bool GeneratesTemporaryValues => true;

    public override ulong Next([NotNull] EntityEntry entry)
    {
        return Interlocked.Decrement(ref _next);
    }
}
