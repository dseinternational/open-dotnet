// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class YesNoUnsureLastSetYesExtensions
{
    public static bool IsNo(this YesNoUnsureLastSetYes value) => value.Value == YesNoUnsure.No;

    public static bool IsYes(this YesNoUnsureLastSetYes value) => value.Value == YesNoUnsure.Yes;

    public static bool IsUnsure(this YesNoUnsureLastSetYes value) => value.Value == YesNoUnsure.Unsure;

    public static bool IsNoOrUnsure(this YesNoUnsureLastSetYes value) => value.IsNo() || value.IsUnsure();
}

