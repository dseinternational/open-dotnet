﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class ComparableValueAttribute : ValueAttribute
{
    public bool EmitNumberStylesFormatting { get; set; }
}
