// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Marks a <see langword="struct"/> for source generation of a comparable value type
/// that wraps an underlying value and provides comparison, equality, formatting and parsing.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class ComparableValueAttribute : ValueAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether the source generator should emit
    /// <see cref="System.Globalization.NumberStyles"/>-based formatting and parsing overloads.
    /// </summary>
    public bool EmitNumberStylesFormatting { get; set; }
}
