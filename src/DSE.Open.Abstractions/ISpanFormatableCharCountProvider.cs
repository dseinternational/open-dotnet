// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public interface ISpanFormatableCharCountProvider : IFormatableCharCountProvider
{
    int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider);
}
