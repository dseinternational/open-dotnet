// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Records;

/// <summary>
/// Extension methods for condition diagnosis code values.
/// </summary>
public static class ConditionCodeExtensions
{
    /// <summary>
    /// Gets the localized label for the specified <see cref="ConditionDiagnosisCode"/>, falling back to the
    /// value's string representation when no label is available.
    /// </summary>
    /// <param name="value">The diagnosis code to look up.</param>
    /// <returns>The label associated with <paramref name="value"/>, or its string representation.</returns>
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
