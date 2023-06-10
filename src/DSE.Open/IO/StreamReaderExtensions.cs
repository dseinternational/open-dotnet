// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.IO;

public static class StreamReaderExtensions
{
    /// <summary>
    /// Sets the position of the stream reader to the specified absolute position.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="position">The position in bytes</param>
    public static void SetPosition(this StreamReader reader, long position)
        => reader.SetPosition(position, SeekOrigin.Begin);

    /// <summary>
    /// Sets the position of the stream reader to the specified position.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="position">The position in bytes</param>
    /// <param name="origin"></param>
    public static void SetPosition(this StreamReader reader, long position, SeekOrigin origin)
    {
        Guard.IsNotNull(reader);
        reader.DiscardBufferedData();
        _ = reader.BaseStream.Seek(position, origin);
    }
}
