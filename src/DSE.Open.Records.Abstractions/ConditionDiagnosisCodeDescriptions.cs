// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;
using DSE.Open.Labels;

namespace DSE.Open.Records;

/// <summary>
/// Provides localized labels and descriptions for <see cref="ConditionDiagnosisCode"/> values.
/// </summary>
public sealed class ConditionDiagnosisCodeDescriptions : ResourceLabelDescriptionProvider<ConditionDiagnosisCode>
{
    private static readonly Lazy<ResourceManager> s_resourceManager = new(()
        => new($"{typeof(ConditionDiagnosisCodeDescriptions).Namespace}.Resources.DiagnosisLabels", typeof(ConditionDiagnosisCodeDescriptions).Assembly));

    /// <summary>
    /// Gets the default <see cref="ConditionDiagnosisCodeDescriptions"/> instance.
    /// </summary>
    public static readonly ConditionDiagnosisCodeDescriptions Default = new();

    private ConditionDiagnosisCodeDescriptions()
    {
    }

    /// <inheritdoc/>
    public override ResourceManager ResourceManager => s_resourceManager.Value;

    /// <inheritdoc/>
    public override string GetLabelKey(ConditionDiagnosisCode value)
    {
        return value.ToString();
    }

    /// <inheritdoc/>
    public override string GetDescriptionKey(ConditionDiagnosisCode value)
    {
        return $"{value}_description";
    }
}
