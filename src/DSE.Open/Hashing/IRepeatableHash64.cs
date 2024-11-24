// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Hashing;

/// <summary>
/// Represents an object that can provide a repeatable hash code derived from its current state.
/// </summary>
public interface IRepeatableHash64
{
    /// <summary>
    /// Gets a 64-bit hash value that is the same on all platforms given equivalent state.
    /// </summary>
    /// <returns>A repeatable 64-bit hash value representing the current state of the object.</returns>
    ulong GetRepeatableHashCode();
}
