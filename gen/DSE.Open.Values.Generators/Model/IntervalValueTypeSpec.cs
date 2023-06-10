// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators.Model;

internal class IntervalValueTypeSpec : OrdinalValueTypeSpec
{
    public override ValueTypeKind ValueTypeKind => ValueTypeKind.Interval;

    public bool EmitAdditionOperator { get; set; } = true;

    public bool EmitDecrementOperator { get; set; } = true;

    public bool EmitIncrementOperator { get; set; } = true;

    public bool EmitSubtractionOperator { get; set; } = true;

    public bool EmitUnaryPlusOperator { get; set; } = true;

    public bool EmitUnaryNegationOperator { get; set; } = true;
}
