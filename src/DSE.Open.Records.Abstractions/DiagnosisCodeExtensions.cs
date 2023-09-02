// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Records.Abstractions;

public static class DiagnosisCodeExtensions
{
    public static string GetLabel(this DiagnosisCode value)
    {
        return DiagnosisCodeDescriptions.Default.GetLabel(value) ?? value.ToString();
    }

    /// <summary>
    /// Indicates if the <see cref="DiagnosisCode"/> value represents one of the codes for Down syndrome.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDownSyndromeDiagnosis(this DiagnosisCode value) =>
        value == DiagnosisCode.DownSyndrome
        || value == DiagnosisCode.DownSyndromeTranslocation
        || value == DiagnosisCode.DownSyndromeMosaic
        || value == DiagnosisCode.DownSyndromePartial;

}
