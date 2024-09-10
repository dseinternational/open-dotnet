// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using DSE.Open.Diagnostics;

namespace DSE.Open;

public static class Utf8StringFormattingExtensions
{
    [SkipLocalsInit]
    public static Utf8String ToUtf8String(this DateTime value, StandardFormat format = default)
    {
        Span<byte> buffer = stackalloc byte[64];

        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
        {
            return new(buffer[..bytesWritten].ToArray());
        }

        Expect.Unreachable("Buffer too small");
        return default; // unreachable
    }

    [SkipLocalsInit]
    public static Utf8String ToUtf8String(this DateTimeOffset value, StandardFormat format = default)
    {
        Span<byte> buffer = stackalloc byte[64];

        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
        {
            return new(buffer[..bytesWritten].ToArray());
        }

        Expect.Unreachable("Buffer too small");
        return default; // unreachable
    }

    [SkipLocalsInit]
    public static Utf8String ToUtf8String(this short value, StandardFormat format = default)
    {
        Span<byte> buffer = stackalloc byte[32];

        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
        {
            return new(buffer[..bytesWritten].ToArray());
        }

        Expect.Unreachable("Buffer too small");
        return default; // unreachable
    }

    [SkipLocalsInit]
    public static Utf8String ToUtf8String(this int value, StandardFormat format = default)
    {
        Span<byte> buffer = stackalloc byte[32];

        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
        {
            return new(buffer[..bytesWritten].ToArray());
        }

        Expect.Unreachable("Buffer too small");
        return default; // unreachable
    }

    [SkipLocalsInit]
    public static Utf8String ToUtf8String(this long value, StandardFormat format = default)
    {
        Span<byte> buffer = stackalloc byte[32];

        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
        {
            return new(buffer[..bytesWritten].ToArray());
        }

        Expect.Unreachable("Buffer too small");
        return default; // unreachable
    }
}
