// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open;

internal static class ValuesMarshal
{
    public static ReadOnlySpan<AsciiChar> AsAsciiChars(ReadOnlySpan<byte> span) => MemoryMarshal.Cast<byte, AsciiChar>(span);

    public static Span<AsciiChar> AsAsciiChars(Span<byte> span) => MemoryMarshal.Cast<byte, AsciiChar>(span);

    public static ReadOnlySpan<byte> AsBytes(ReadOnlySpan<AsciiChar> span) => MemoryMarshal.AsBytes(span);

    public static Span<byte> AsBytes(Span<AsciiChar> span) => MemoryMarshal.AsBytes(span);
}
