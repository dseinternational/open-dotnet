// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Predicate helpers for testing the choice represented by a <see cref="YesNo"/> value.
/// </summary>
public static class YesNoExtensions
{
    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> equals <see cref="YesNo.No"/>.</summary>
    public static bool IsNo(this YesNo value)
    {
        return value == YesNo.No;
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> equals <see cref="YesNo.Yes"/>.</summary>
    public static bool IsYes(this YesNo value)
    {
        return value == YesNo.Yes;
    }
}
