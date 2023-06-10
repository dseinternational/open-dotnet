// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators.Model;

internal class DivisibleValueTypeGenerationSpec : AddableValueTypeSpec
{
    public override ValueTypeKind ValueTypeKind => ValueTypeKind.Divisible;

    public bool EmitMultiplicationOperator { get; set; } = true;

    public bool EmitDivisionOperator { get; set; } = true;

    public bool EmitModulusOperator { get; set; } = true;
}
