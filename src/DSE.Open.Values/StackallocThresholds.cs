// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

internal static class StackallocThresholds
{
    public const int MaxByteLength = 256;

    public const int MaxCharLength = MaxByteLength / 2;
}
