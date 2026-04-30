// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="DiagnosticCode"/> values as JSON strings.
/// </summary>
public class JsonStringDiagnosticCodeConverter : SpanParsableCharWritingJsonConverter<DiagnosticCode>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringDiagnosticCodeConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(DiagnosticCode value)
    {
        return DiagnosticCode.MaxLength;
    }
}
