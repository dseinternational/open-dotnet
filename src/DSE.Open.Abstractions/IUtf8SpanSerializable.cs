// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

/// <summary>
/// Indicates that a type can be written to and read from a span of bytes.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IUtf8SpanSerializable<TSelf> : IUtf8SpanParsable<TSelf>, IUtf8SpanFormattable
    where TSelf : IUtf8SpanSerializable<TSelf>
{
    /// <summary>
    /// Gets the maximum number of bytes that a value can be serialized to.
    /// </summary>
    static abstract int MaxSerializedByteLength { get; }
}
