// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Marks a <see langword="struct"/> for source generation of an equatable value type
/// that wraps an underlying value and provides equality, formatting and parsing.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class EquatableValueAttribute : ValueAttribute;
