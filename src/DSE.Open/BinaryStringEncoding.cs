// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Identifies the textual encoding used to represent a sequence of bytes as a string.
/// </summary>
public enum BinaryStringEncoding
{
    /// <summary>
    /// Standard RFC 4648 Base64 encoding.
    /// </summary>
    Base64,

    /// <summary>
    /// Lowercase hexadecimal encoding (for example, <c>"0f1a"</c>).
    /// </summary>
    HexLower,

    /// <summary>
    /// Uppercase hexadecimal encoding (for example, <c>"0F1A"</c>).
    /// </summary>
    HexUpper,

    /// <summary>
    /// Base62 encoding using the alphabet <c>0-9A-Za-z</c>.
    /// </summary>
    Base62
}
