// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Marks a <see langword="struct"/> for source generation of an addable value type
/// that wraps an underlying value and provides arithmetic addition, subtraction,
/// increment and decrement operations.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class AddableValueAttribute : ValueAttribute;
