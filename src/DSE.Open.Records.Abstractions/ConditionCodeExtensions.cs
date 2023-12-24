// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Records;

public static class ConditionCodeExtensions
{
    public static string GetLabel(this ConditionDiagnosisCode value)
    {
        return ConditionDiagnosisCodeDescriptions.Default.GetLabel(value, null) ?? value.ToString();
    }

    /// <summary>
    /// Indicates if the <see cref="ConditionDiagnosisCode"/> value represents one of the codes for Down syndrome.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDownSyndromeDiagnosis(this ConditionDiagnosisCode value)
    {
        return value == ConditionDiagnosisCode.DownSyndrome
               || value == ConditionDiagnosisCode.DownSyndromeTranslocation
               || value == ConditionDiagnosisCode.DownSyndromeMosaic
               || value == ConditionDiagnosisCode.DownSyndromePartial;
    }
}
