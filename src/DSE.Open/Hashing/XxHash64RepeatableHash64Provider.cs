// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;

namespace DSE.Open.Hashing;

public class XxHash64RepeatableHash64Provider : RepeatableHash64Provider
{
    protected override ulong GetRepeatableHashCodeCore(ReadOnlySpan<byte> value)
    {
        return XxHash64.HashToUInt64(value);
    }
}
