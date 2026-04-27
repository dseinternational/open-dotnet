// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="ICollection{T}"/> of <see cref="string"/> to and from a
/// comma-separated string.
/// </summary>
public sealed class StringCollectionToCommaSeparatedStringConverter : ValueConverter<ICollection<string>, string>
{
    public static readonly StringCollectionToCommaSeparatedStringConverter Default = new();

    public StringCollectionToCommaSeparatedStringConverter()
        : base(v => ConvertToString(v), v => ConvertToCollection(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(ICollection<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return string.Join(',', values);
    }

    // keep public for EF Core compiled models
    public static ICollection<string> ConvertToCollection(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return [.. value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
    }
}
