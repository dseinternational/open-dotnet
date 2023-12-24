// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringDiagnosticCodeConverter : SpanParsableCharWritingJsonConverter<DiagnosticCode>
{
    public static readonly JsonStringDiagnosticCodeConverter Default = new();

    protected override int GetMaxCharCountToWrite(DiagnosticCode value)
    {
        return DiagnosticCode.MaxLength;
    }
}
