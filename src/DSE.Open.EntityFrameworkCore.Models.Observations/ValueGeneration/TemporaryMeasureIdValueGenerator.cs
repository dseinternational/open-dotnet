// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.ValueGeneration;

/// <summary>
/// Generates <see cref="MeasureId"/> values safe for use with <c>bigint</c> (<see cref="long"/>) columns.
/// </summary>
public sealed class TemporaryMeasureIdValueGenerator : ValueGenerator<MeasureId>
{
    private ulong _next = MeasureId.MaxIdValue;

    public override bool GeneratesTemporaryValues => true;

    public override MeasureId Next([NotNull] EntityEntry entry)
    {
        return new(Interlocked.Decrement(ref _next));
    }
}
