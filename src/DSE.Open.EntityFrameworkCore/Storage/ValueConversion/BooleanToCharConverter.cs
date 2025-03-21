// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class BooleanToCharConverter : ValueConverter<bool, char>
{
    public static readonly BooleanToCharConverter Default = new();

    public BooleanToCharConverter()
        : base(v => ConvertToChar(v), v => ConvertToBoolean(v))
    {
    }

    // keep public for EF Core compiled models
    public static char ConvertToChar(bool value)
    {
        return value ? 'Y' : 'N';
    }

    // keep public for EF Core compiled models
    public static bool ConvertToBoolean(char value)
    {
        return value == 'Y';
    }
}
