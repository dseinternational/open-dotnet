// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public interface ISpanFormattableCharCountProvider : IFormattableCharCountProvider
{
    int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider);
}

[Obsolete("Renamed to " + nameof(ISpanFormattableCharCountProvider) + ". This alias will be removed in a future release.")]
public interface ISpanFormatableCharCountProvider : ISpanFormattableCharCountProvider
{
}
