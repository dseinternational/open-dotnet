// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Provides a <see cref="YesNoUnsure"/> response together with the date and time
/// when the response was last marked <see cref="YesNoUnsure.Yes"/>.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct YesNoUnsureLastSetYes(

    [property: JsonPropertyName("value")]
    YesNoUnsure Value,

    [property: JsonPropertyName("last_set_yes")]
    DateTimeOffset? LastSetYes)
{
    public static YesNoUnsureLastSetYes FromYesNoUnsure(YesNoUnsure value, DateTimeOffset? lastSetYes = null)
    {
        return new YesNoUnsureLastSetYes
        {
            Value = value,
            LastSetYes = lastSetYes
        };
    }

    public static explicit operator YesNoUnsureLastSetYes(YesNoUnsure value)
    {
        return FromYesNoUnsure(value);
    }

    public static YesNoUnsureLastSetYes YesNow => new() { Value = YesNoUnsure.Yes, LastSetYes = DateTimeOffset.Now };

    public static YesNoUnsureLastSetYes NoNow => new() { Value = YesNoUnsure.No, LastSetYes = DateTimeOffset.Now };
}
