// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators;

internal static class TypeNames
{
    public const string ValueTypesNamespace = $"DSE.Open.Values";

    public const string NominalValueAttributeName = "NominalValueAttribute";
    public const string OrdinalValueAttributeName = "OrdinalValueAttribute";
    public const string IntervalValueAttributeName = "IntervalValueAttribute";
    public const string RatioValueAttributeName = "RatioValueAttribute";

    public const string NominalValueAttributeFullName = $"DSE.Open.Values.{NominalValueAttributeName}";
    public const string OrdinalValueAttributeFullName = $"DSE.Open.Values.{OrdinalValueAttributeName}";
    public const string IntervalValueAttributeFullName = $"DSE.Open.Values.{IntervalValueAttributeName}";
    public const string RatioValueAttributeFullName = $"DSE.Open.Values.{RatioValueAttributeName}";

    public const string INominalValueInterfaceName = "INominalValue";
    public const string IOrdinalValueInterfaceName = "IOrdinalValue";
    public const string IntervalValueInterfaceName = "IIntervalValue";
    public const string IRatioValueInterfaceName = "IRatioValue";
}
