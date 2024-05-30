// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

/// <summary>
/// Codes used to record diagnoses of conditions in our systems.
/// </summary>
/// <remarks>
/// By default, values are serialized to strings as 18-digit integers are greater than
/// JavaScript Number.MAX_SAFE_INTEGER and will be truncated via JSON.parse().
/// </remarks>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<ConditionDiagnosisCode, ClinicalConceptCode>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct ConditionDiagnosisCode : IEquatableValue<ConditionDiagnosisCode, ClinicalConceptCode>, IUtf8SpanSerializable<ConditionDiagnosisCode>
{
    public static int MaxSerializedCharLength => ClinicalConceptCode.MaxSerializedCharLength;

    public static int MaxSerializedByteLength => ClinicalConceptCode.MaxSerializedByteLength;

    public static bool IsValidValue(ClinicalConceptCode value)
    {
        return Lookup.ContainsKey(value);
    }

    public static implicit operator ConditionDiagnosisCode(long code)
    {
        return FromInt64(code);
    }

    public static ConditionDiagnosisCode FromInt32(int code)
    {
        return FromInt64(code);
    }

    public static ConditionDiagnosisCode FromInt64(long code)
    {
        return new(new(code));
    }

    /// <summary>
    /// Identifies a diagnosis of Down syndrome [41040004 | Complete trisomy 21 syndrome (disorder)].
    /// May be complete, mosaic, translocation or partial.
    /// </summary>
    public static readonly ConditionDiagnosisCode DownSyndrome = new((ClinicalConceptCode)41040004, true);

    /// <summary>
    /// Identifies a diagnosis of translocation Down syndrome [371045000 | Translocation Down syndrome (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode DownSyndromeTranslocation = new((ClinicalConceptCode)371045000, true);

    /// <summary>
    /// Identifies a diagnosis of mosaic Down syndrome [205616004 | Trisomy 21 - mitotic nondisjunction mosaicism (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode DownSyndromeMosaic = new((ClinicalConceptCode)205616004, true);

    /// <summary>
    /// Identifies a diagnosis of mosaic Down syndrome [254264002 | Partial trisomy 21 in Down syndrome (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode DownSyndromePartial = new((ClinicalConceptCode)254264002, true);

    /// <summary>
    /// Identifies a diagnosis of Fragile X syndrome [613003 | Fragile X syndrome (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode FragileX = new((ClinicalConceptCode)613003, true);

    /// <summary>
    /// Identifies a diagnosis of Autism spectrum disorder [35919005 | Pervasive developmental disorder (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode AutismSpectrumDisorder = new((ClinicalConceptCode)35919005, true);

    /// <summary>
    /// Identifies a diagnosis of Williams syndrome [63247009 | Williams syndrome (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode WilliamsSyndrome = new((ClinicalConceptCode)63247009, true);

    /// <summary>
    /// Identifies a diagnosis of Developmental language disorder [280032002 | Developmental language disorder (disorder)].
    /// </summary>
    public static readonly ConditionDiagnosisCode DevelopmentalLanguageDisorder = new((ClinicalConceptCode)280032002, true);

    /// <summary>
    /// Identifies a diagnosis of Specific language impairment [229746007 | Specific language impairment (finding)].
    /// </summary>
    public static readonly ConditionDiagnosisCode SpecificLanguageImpairment = new((ClinicalConceptCode)229746007, true);

    public static readonly IReadOnlyCollection<ConditionDiagnosisCode> All =
    [
        DownSyndrome,
        DownSyndromeMosaic,
        DownSyndromePartial,
        DownSyndromeTranslocation,
        FragileX,
        AutismSpectrumDisorder,
        WilliamsSyndrome,
        DevelopmentalLanguageDisorder,
        SpecificLanguageImpairment,
    ];

    public static readonly IReadOnlyDictionary<ClinicalConceptCode, ConditionDiagnosisCode> Lookup = All.ToDictionary(r => r._value);
}
