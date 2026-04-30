// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="SemanticRelation"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class SemanticRelationToStringConverter : ValueConverter<SemanticRelation, string>
{
    /// <summary>
    /// Gets the default <see cref="SemanticRelationToStringConverter"/> instance.
    /// </summary>
    public static readonly SemanticRelationToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SemanticRelationToStringConverter"/> class.
    /// </summary>
    public SemanticRelationToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="SemanticRelation"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertToString(SemanticRelation code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="SemanticRelation"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static SemanticRelation ConvertFromString(string code)
    {
        if (SemanticRelation.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SemanticRelation)}");
        return default; // unreachable
    }
}
