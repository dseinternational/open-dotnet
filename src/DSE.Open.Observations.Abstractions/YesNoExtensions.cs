// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class YesNoExtensions
{
    public static bool IsNo(this YesNo value) => value == YesNo.No;

    public static bool IsYes(this YesNo value) => value == YesNo.Yes;
}
