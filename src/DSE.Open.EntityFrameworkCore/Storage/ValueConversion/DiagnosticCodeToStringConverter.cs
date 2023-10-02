// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class DiagnosticCodeToStringConverter : ValueConverter<Diagnostics.DiagnosticCode, string>
{
    public static readonly DiagnosticCodeToStringConverter Default = new();

    public DiagnosticCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(Open.Diagnostics.DiagnosticCode code) => code.ToString();

    private static Open.Diagnostics.DiagnosticCode ConvertFromString(string code)
    {
        if (Diagnostics.DiagnosticCode.TryParse(code, out var value))
        {
            return value;
        }

        ValueConversionException.Throw($"Error converting string value '{code}' to DiagnosticCode.", code, null);
        return default;
    }
}
