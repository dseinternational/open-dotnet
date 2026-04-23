// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Marks a <see langword="struct"/> for source generation of a divisible value type
/// that wraps an underlying value and provides division, multiplication and modulus
/// operations in addition to those provided by <see cref="AddableValueAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class DivisibleValueAttribute : ValueAttribute;
