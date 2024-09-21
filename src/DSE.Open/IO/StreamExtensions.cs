// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.IO;

public static class StreamExtensions
{
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

    public static async Task<byte[]> ReadToEndAsync(this Stream stream, int initialLength = 0)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (initialLength < 1)
        {
            initialLength = 1024 * 4;
        }

        var buffer = new byte[initialLength];
        var read = 0;

        int chunk;

        while ((chunk = await stream.ReadAsync(buffer.AsMemory(read, buffer.Length - read)).ConfigureAwait(false)) > 0)
        {
            read += chunk;

            if (read == buffer.Length)
            {
                var nextByte = stream.ReadByte();

                if (nextByte == -1)
                {
                    return [.. buffer];
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
}
