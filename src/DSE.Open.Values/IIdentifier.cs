// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

#pragma warning disable CA1716 // Identifiers should not match keywords

/// <summary>
/// Defines a type that serves as a unique identifier and can generate new identifier values.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
public interface IIdentifier<TSelf>
    : IEquatable<TSelf>,
      ISpanFormattable
    where TSelf : IIdentifier<TSelf>
{
    /// <summary>Generates a new unique identifier value.</summary>
    static abstract TSelf New();
}
