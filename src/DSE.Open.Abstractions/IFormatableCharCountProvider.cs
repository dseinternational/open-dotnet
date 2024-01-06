﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public interface IFormatableCharCountProvider
{
    int GetCharCount(string? format, IFormatProvider? formatProvider);
}
