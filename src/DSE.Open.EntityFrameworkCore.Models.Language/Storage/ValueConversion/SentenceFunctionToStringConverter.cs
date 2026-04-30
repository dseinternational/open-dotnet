// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="SentenceFunction"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class SentenceFunctionToStringConverter : ValueConverter<SentenceFunction, string>
{
    /// <summary>
    /// Gets the default <see cref="SentenceFunctionToStringConverter"/> instance.
    /// </summary>
    public static readonly SentenceFunctionToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SentenceFunctionToStringConverter"/> class.
    /// </summary>
    public SentenceFunctionToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="SentenceFunction"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertToString(SentenceFunction code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="SentenceFunction"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static SentenceFunction ConvertFromString(string code)
    {
        if (SentenceFunction.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SentenceFunction)}");
        return default; // unreachable
    }
}
