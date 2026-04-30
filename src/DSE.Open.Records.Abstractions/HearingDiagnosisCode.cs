// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

/// <summary>
/// Codes used to record diagnoses of hearing disorders in our systems.
/// </summary>
/// <remarks>
/// By default, values are serialized to strings as 18-digit integers are greater than
/// JavaScript Number.MAX_SAFE_INTEGER and will be truncated via JSON.parse().
/// </remarks>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<HearingDiagnosisCode, ClinicalConceptCode>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct HearingDiagnosisCode : IEquatableValue<HearingDiagnosisCode, ClinicalConceptCode>, IUtf8SpanSerializable<HearingDiagnosisCode>
{
    /// <inheritdoc/>
    public static int MaxSerializedCharLength => ClinicalConceptCode.MaxSerializedCharLength;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => ClinicalConceptCode.MaxSerializedByteLength;

    /// <inheritdoc/>
    public static bool IsValidValue(ClinicalConceptCode value)
    {
        return Lookup.ContainsKey(value);
    }

    /// <summary>
    /// Implicitly converts a <see cref="long"/> SNOMED CT Concept ID to a <see cref="HearingDiagnosisCode"/>.
    /// </summary>
    /// <param name="code">The SNOMED CT Concept ID.</param>
    public static implicit operator HearingDiagnosisCode(long code)
    {
        return FromInt64(code);
    }

    /// <summary>
    /// Creates a <see cref="HearingDiagnosisCode"/> from a 32-bit SNOMED CT Concept ID.
    /// </summary>
    /// <param name="code">The SNOMED CT Concept ID.</param>
    public static HearingDiagnosisCode FromInt32(int code)
    {
        return FromInt64(code);
    }

    /// <summary>
    /// Creates a <see cref="HearingDiagnosisCode"/> from a 64-bit SNOMED CT Concept ID.
    /// </summary>
    /// <param name="code">The SNOMED CT Concept ID.</param>
    public static HearingDiagnosisCode FromInt64(long code)
    {
        return new(new(code));
    }

    /// <summary>
    /// Identifies a diagnosis of Hearing loss [15188001 | Hearing loss (disorder)].
    /// </summary>
    public static readonly HearingDiagnosisCode HearingLoss = new((ClinicalConceptCode)15188001, true);

    /// <summary>
    /// Identifies a diagnosis of Mixed conductive AND sensorineural hearing loss [77507001 | Mixed conductive AND sensorineural hearing loss (disorder)].
    /// </summary>
    public static readonly HearingDiagnosisCode ConductiveAndSensorineuralHearingLoss = new((ClinicalConceptCode)77507001, true);

    /// <summary>
    /// Identifies a diagnosis of Otitis media [65363002 | Otitis media (disorder)].
    /// </summary>
    public static readonly HearingDiagnosisCode OtitisMedia = new((ClinicalConceptCode)65363002, true);

    /// <summary>
    /// Identifies a diagnosis of Otitis media with effusion [80327007 | Serous otitis media (disorder)].
    /// </summary>
    public static readonly HearingDiagnosisCode OtitisMediaWithEffusion = new((ClinicalConceptCode)80327007, true);

    /// <summary>
    /// Identifies a diagnosis of Sensorineural hearing loss [60700002 | Sensorineural hearing loss (disorder)]
    /// </summary>
    public static readonly HearingDiagnosisCode SensorineuralHearingLoss = new((ClinicalConceptCode)60700002, true);


    /// <summary>
    /// Gets all defined <see cref="HearingDiagnosisCode"/> values.
    /// </summary>
    public static readonly IReadOnlyCollection<HearingDiagnosisCode> All =
    [
        HearingLoss,
        ConductiveAndSensorineuralHearingLoss,
        OtitisMedia,
        OtitisMediaWithEffusion,
        SensorineuralHearingLoss,
    ];

    /// <summary>
    /// Gets a lookup of defined <see cref="HearingDiagnosisCode"/> values keyed by their underlying
    /// <see cref="ClinicalConceptCode"/>.
    /// </summary>
    public static readonly IReadOnlyDictionary<ClinicalConceptCode, HearingDiagnosisCode> Lookup = All.ToDictionary(r => r._value);
}
