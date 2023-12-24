// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class YesNoUnsureExtensions
{
    public static bool IsNo(this YesNoUnsure value)
    {
        return value == YesNoUnsure.No;
    }

    public static bool IsYes(this YesNoUnsure value)
    {
        return value == YesNoUnsure.Yes;
    }

    public static bool IsUnsure(this YesNoUnsure value)
    {
        return value == YesNoUnsure.Unsure;
    }

    public static bool IsNoOrUnsure(this YesNoUnsure value)
    {
        return value.IsNo() || value.IsUnsure();
    }
}
