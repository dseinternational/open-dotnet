// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Text;

namespace DSE.Open.Observations;

public static class MeasureIdHelper
{
    public static uint GetId(Uri urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        return GetId(urn.ToString());
    }

    public static uint GetId(ReadOnlySpan<char> urn)
    {
        var c = Encoding.UTF8.GetByteCount(urn);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(urn, b);
        return (uint)(XxHash3.HashToUInt64(b) / (double)ulong.MaxValue * uint.MaxValue);
    }
}
