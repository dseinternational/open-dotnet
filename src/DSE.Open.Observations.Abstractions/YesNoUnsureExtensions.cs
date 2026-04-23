// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Predicate helpers for testing the choice represented by a <see cref="YesNoUnsure"/> value.
/// </summary>
public static class YesNoUnsureExtensions
{
    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> equals <see cref="YesNoUnsure.No"/>.</summary>
    public static bool IsNo(this YesNoUnsure value)
    {
        return value == YesNoUnsure.No;
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> equals <see cref="YesNoUnsure.Yes"/>.</summary>
    public static bool IsYes(this YesNoUnsure value)
    {
        return value == YesNoUnsure.Yes;
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> equals <see cref="YesNoUnsure.Unsure"/>.</summary>
    public static bool IsUnsure(this YesNoUnsure value)
    {
        return value == YesNoUnsure.Unsure;
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="value"/> is either
    /// <see cref="YesNoUnsure.No"/> or <see cref="YesNoUnsure.Unsure"/> —
    /// useful when treating any non-affirmative response as a single category.
    /// </summary>
    public static bool IsNoOrUnsure(this YesNoUnsure value)
    {
        return value.IsNo() || value.IsUnsure();
    }
}
