// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public interface IFormattableCharCountProvider
{
    int GetCharCount(string? format, IFormatProvider? formatProvider);
}

[Obsolete("Renamed to " + nameof(IFormattableCharCountProvider) + ". This alias will be removed in a future release.")]
public interface IFormatableCharCountProvider : IFormattableCharCountProvider
{
}
