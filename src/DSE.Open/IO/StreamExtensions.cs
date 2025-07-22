// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;

namespace DSE.Open.IO;

#pragma warning disable DSEOPEN001 // ArrayBuilder

public static class StreamExtensions
{
    public static byte[] ReadToEnd(this Stream stream)
    {
        return ReadToEnd(stream, 0);
    }

    public static byte[] ReadToEnd(this Stream stream, int initialLength = 0)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (initialLength < 1)
        {
            initialLength = 1024 * 4;
        }

        var buffer = new byte[initialLength];
        var read = 0;

        int chunk;

        while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
        {
            read += chunk;

            if (read == buffer.Length)
            {
                var nextByte = stream.ReadByte();

                if (nextByte == -1)
                {
                    return buffer;
                }

                var newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }
        }

        var result = new byte[read];
        Array.Copy(buffer, result, read);
        return result;
    }

    public static Task<byte[]> ReadToEndAsync(
        this Stream stream,
        CancellationToken cancellationToken = default)
    {
        return ReadToEndAsync(stream, 0, cancellationToken);
    }

    public static async Task<byte[]> ReadToEndAsync(
        this Stream stream,
        int initialLength,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (initialLength < 1)
        {
            initialLength = 1024 * 4;
        }

        var buffer = ArrayPool<byte>.Shared.Rent(initialLength);

        try
        {
            var read = 0;

            int chunk;

            while ((chunk = await stream.ReadAsync(
                buffer.AsMemory(read, buffer.Length - read), cancellationToken).ConfigureAwait(false))
                > 0)
            {
                read += chunk;

                if (read == buffer.Length)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var newBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length * 2);
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    ArrayPool<byte>.Shared.Return(buffer);
                    buffer = newBuffer;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (read == 0)
            {
                return [];
            }

            var result = new byte[read];
            Array.Copy(buffer, result, read);
            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static string ReadToEndAsString(this Stream stream)
    {
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static string ReadToEndAsString(this Stream stream, Encoding encoding)
    {
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static Task<string> ReadToEndAsStringAsync(this Stream stream)
    {
        return ReadToEndAsStringAsync(stream, Encoding.UTF8);
    }

    public static async Task<string> ReadToEndAsStringAsync(this Stream stream, Encoding encoding)
    {
        using var reader = new StreamReader(stream, encoding);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    public static void Write(this Stream stream, in ReadOnlySequence<byte> sequence)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (sequence.IsSingleSegment)
        {
            stream.Write(sequence.FirstSpan);
            return;
        }

        foreach (var segment in sequence)
        {
            stream.Write(segment.Span);
        }
    }

    public static async ValueTask WriteAsync(
        this Stream stream,
        ReadOnlySequence<byte> sequence,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (sequence.IsSingleSegment)
        {
            await stream.WriteAsync(sequence.First, cancellationToken).ConfigureAwait(false);
            return;
        }

        foreach (var segment in sequence)
        {
            await stream.WriteAsync(segment, cancellationToken).ConfigureAwait(false);
        }
    }
}
