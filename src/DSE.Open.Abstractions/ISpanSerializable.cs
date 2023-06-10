// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

/// <summary>
/// Indicates that a type can be written to and read from a span of characters.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface ISpanSerializable<TSelf> : ISpanParsable<TSelf>, ISpanFormattable
    where TSelf : ISpanSerializable<TSelf>
{
    /// <summary>
    /// Gets the maximum number of characters that a value of type
    /// <typeparamref name="TSelf"/> may be serialized to.
    /// </summary>
    static abstract int MaxSerializedCharLength { get; }
}
