// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.HighPerformance.Buffers;

namespace DSE.Open.Language;

internal static class PosTagStringPool
{
    public static readonly StringPool Shared = new();
}
