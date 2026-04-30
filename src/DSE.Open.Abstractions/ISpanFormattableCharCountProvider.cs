// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Provides the number of characters required to format a value when given a span format specifier.
/// </summary>
public interface ISpanFormattableCharCountProvider : IFormattableCharCountProvider
{
    /// <summary>
    /// Returns the number of characters required to format the value using the given span format and format provider.
    /// </summary>
    int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider);
}

/// <summary>
/// Obsolete alias for <see cref="ISpanFormattableCharCountProvider"/>.
/// </summary>
[Obsolete("Renamed to " + nameof(ISpanFormattableCharCountProvider) + ". This alias will be removed in a future release.")]
public interface ISpanFormatableCharCountProvider : ISpanFormattableCharCountProvider
{
}
