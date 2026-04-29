// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;

namespace DSE.Open.Hashing;

/// <summary>
/// A <see cref="RepeatableHash64Provider"/> that produces 64-bit hashes using the XxHash64 algorithm.
/// </summary>
public class XxHash64RepeatableHash64Provider : RepeatableHash64Provider
{
    /// <inheritdoc/>
    protected override ulong GetRepeatableHashCodeCore(ReadOnlySpan<byte> value)
    {
        return XxHash64.HashToUInt64(value);
    }
}
