// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;

namespace DSE.Open.IO;

/// <summary>
/// Provides extension methods for <see cref="Stream"/>.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Reads the stream to its end and returns the bytes read as a new array.
    /// </summary>
    public static byte[] ReadToEnd(this Stream stream)
    {
        return ReadToEnd(stream, 0);
    }

    /// <summary>
    /// Reads the stream to its end and returns the bytes read as a new array,
    /// using the specified initial buffer length (a default size is used when
    /// <paramref name="initialLength"/> is less than 1).
    /// </summary>
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

    /// <summary>
    /// Asynchronously reads the stream to its end and returns the bytes read as a new array.
    /// </summary>
    public static Task<byte[]> ReadToEndAsync(
        this Stream stream,
        CancellationToken cancellationToken = default)
    {
        return ReadToEndAsync(stream, 0, cancellationToken);
    }

    /// <summary>
    /// Asynchronously reads the stream to its end and returns the bytes read as a new array,
    /// using the specified initial buffer length (a default size is used when
    /// <paramref name="initialLength"/> is less than 1).
    /// </summary>
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

    /// <summary>
    /// Reads the stream to its end and returns the result decoded as a string,
    /// detecting the encoding from byte order marks where present.
    /// </summary>
    public static string ReadToEndAsString(this Stream stream)
    {
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Reads the stream to its end and returns the result decoded as a string
    /// using the specified <paramref name="encoding"/>.
    /// </summary>
    public static string ReadToEndAsString(this Stream stream, Encoding encoding)
    {
        using var reader = new StreamReader(stream, encoding);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Asynchronously reads the stream to its end and returns the result decoded as a UTF-8 string.
    /// </summary>
    public static Task<string> ReadToEndAsStringAsync(this Stream stream)
    {
        return ReadToEndAsStringAsync(stream, Encoding.UTF8);
    }

    /// <summary>
    /// Asynchronously reads the stream to its end and returns the result decoded as a string
    /// using the specified <paramref name="encoding"/>.
    /// </summary>
    public static async Task<string> ReadToEndAsStringAsync(this Stream stream, Encoding encoding)
    {
        using var reader = new StreamReader(stream, encoding);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Writes the contents of the supplied <see cref="ReadOnlySequence{T}"/> to the stream.
    /// </summary>
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

    /// <summary>
    /// Asynchronously writes the contents of the supplied <see cref="ReadOnlySequence{T}"/> to the stream.
    /// </summary>
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
