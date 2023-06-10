// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators.Model;

internal class OrdinalValueTypeSpec : NominalValueTypeSpec
{
    public override ValueTypeKind ValueTypeKind => ValueTypeKind.Ordinal;

    public bool EmitCompareToMethod { get; set; } = true;

    public bool EmitCompareToObjectMethod { get; set; } = true;

    public bool EmitComparisonOperators { get; set; } = true;
}
