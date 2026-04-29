// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="SentenceStructure"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class SentenceStructureToStringConverter : ValueConverter<SentenceStructure, string>
{
    /// <summary>
    /// Gets the default <see cref="SentenceStructureToStringConverter"/> instance.
    /// </summary>
    public static readonly SentenceStructureToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SentenceStructureToStringConverter"/> class.
    /// </summary>
    public SentenceStructureToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="SentenceStructure"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertToString(SentenceStructure code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="SentenceStructure"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static SentenceStructure ConvertFromString(string code)
    {
        if (SentenceStructure.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SentenceStructure)}");
        return default; // unreachable
    }
}
