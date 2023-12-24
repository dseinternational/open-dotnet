// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

/// <summary>
/// A SNOMED CT Concept ID.
/// </summary>
/// <remarks>
/// By default, values are serialized to strings as 18-digit integers are greater than
/// JavaScript Number.MAX_SAFE_INTEGER and will be truncated via JSON.parse().
/// </remarks>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<ClinicalConceptCode, long>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct ClinicalConceptCode : IEquatableValue<ClinicalConceptCode, long>, IClinicalConceptCode<ClinicalConceptCode>, IUtf8SpanSerializable<ClinicalConceptCode>
{
    public static int MaxSerializedCharLength => 8;

    public static int MaxSerializedByteLength => 8;

    public ClinicalConceptCode(long code) : this(code, false)
    {
    }

    public static bool IsValidValue(long value)
    {
        return value is >= 100000 and <= 999999999999999999;
    }
}
