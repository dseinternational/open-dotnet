// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Provides the number of characters required to format a value as a string.
/// </summary>
public interface IFormattableCharCountProvider
{
    /// <summary>
    /// Returns the number of characters required to format the value using the given format and format provider.
    /// </summary>
    int GetCharCount(string? format, IFormatProvider? formatProvider);
}

/// <summary>
/// Obsolete alias for <see cref="IFormattableCharCountProvider"/>.
/// </summary>
[Obsolete("Renamed to " + nameof(IFormattableCharCountProvider) + ". This alias will be removed in a future release.")]
public interface IFormatableCharCountProvider : IFormattableCharCountProvider
{
}
