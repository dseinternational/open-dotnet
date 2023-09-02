// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;
using DSE.Open.Labels;

namespace DSE.Open.Records.Abstractions;

public sealed class DiagnosisCodeDescriptions : ResourceLabelDescriptionProvider<DiagnosisCode>
{
    private static readonly Lazy<ResourceManager> s_resourceManager = new(()
        => new ResourceManager($"{typeof(DiagnosisCodeDescriptions).Namespace}.Resources.DiagnosisLabels", typeof(DiagnosisCodeDescriptions).Assembly));

    public static readonly DiagnosisCodeDescriptions Default = new();

    private DiagnosisCodeDescriptions()
    {
    }

    public override ResourceManager ResourceManager => s_resourceManager.Value;

    public override string GetLabelKey(DiagnosisCode value)
    {
        return value.ToString();
    }

    public override string GetDescriptionKey(DiagnosisCode value)
    {
        return "{value}_description";
    }
}
