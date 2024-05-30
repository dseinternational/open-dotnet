// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;
using DSE.Open.Labels;

namespace DSE.Open.Records;

public sealed class ConditionDiagnosisCodeDescriptions : ResourceLabelDescriptionProvider<ConditionDiagnosisCode>
{
    private static readonly Lazy<ResourceManager> s_resourceManager = new(()
        => new($"{typeof(ConditionDiagnosisCodeDescriptions).Namespace}.Resources.DiagnosisLabels", typeof(ConditionDiagnosisCodeDescriptions).Assembly));

    public static readonly ConditionDiagnosisCodeDescriptions Default = new();

    private ConditionDiagnosisCodeDescriptions()
    {
    }

    public override ResourceManager ResourceManager => s_resourceManager.Value;

    public override string GetLabelKey(ConditionDiagnosisCode value)
    {
        return value.ToString();
    }

    public override string GetDescriptionKey(ConditionDiagnosisCode value)
    {
        return "{value}_description";
    }
}
