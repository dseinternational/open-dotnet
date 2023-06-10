// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace DSE.Open.Values.Generators.Model;

internal abstract class ValueTypeSpec
{
    public abstract ValueTypeKind ValueTypeKind { get; }

    public string ValueTypeName { get; set; } = default!;

    public string Namespace { get; set; } = default!;

    public Accessibility Accessibility { get; set; } = Accessibility.Public;

    public ParentClass? ParentClass { get; set; }

    public IEnumerable<string> Fields { get; set; } = Array.Empty<string>();

    public string ContainedValueTypeName { get; set; } = default!;

    public string ImplementedInterface => ValueTypeKind switch
    {
        ValueTypeKind.Nominal => $"{TypeNames.INominalValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Ordinal => $"{TypeNames.IOrdinalValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Interval => $"{TypeNames.IntervalValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Ratio => $"{TypeNames.IRatioValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        _ => throw new NotSupportedException(),
    };

    public bool EmitValueField { get; set; } = true;

    public string ValueFieldName { get; set; } = "_value";

    public string DefaultValueFieldName { get; set; } = "s_defaultValue";

    /// <summary>
    /// Indicates if the default value field should be used for equality, comparisons, formatting and
    /// parsing. If enabled, a static readonly field with the name specified in
    /// <see cref="DefaultValueFieldName"/> should be declared by the user and assigned an appropriate
    /// value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When enabled, the value assigned will be:
    /// • treated as equal to the value <c>(T)default</c> and its own value;
    /// • used for formatting if the current value is <c>(T)default</c>
    /// • recognised when parsing and result in the assignment of <c>(T)default</c>
    /// </para>
    /// </remarks>
    public bool UseDefaultValueField { get; set; }

    /// <summary>
    /// Indicates if a <c>private static string GetString(string s)</c> method should be called from
    /// formatting methods to support interning and/or pooling of strings.
    /// </summary>
    public bool UseGetString { get; set; }

    /// <summary>
    /// Indicates if a <c>private static string GetString(ReadOnlySpan&lt;char&gt; s)</c>
    /// methods should be called from formatting methods to support pooling of strings.
    /// </summary>
    public bool UseGetStringSpan { get; set; }

    public bool EmitConstructor { get; set; } = true;

    public bool EmitMaxSerializedCharLength => MaxSerializedCharLength > 0;

    public int MaxSerializedCharLength { get; set; }

    // ITryConvertibleFrom<TSelf, T>

    public bool EmitEnsureIsValidValueMethod { get; set; } = true;

    public bool EmitTryFromValueMethod { get; set; } = true;

    public bool EmitFromValueMethod { get; set; } = true;

    public bool EmitExplicitConversionFromContainedTypeMethod { get; set; } = true;

    // IConvertibleTo<TSelf, T>

    public bool EmitConvertToMethod { get; set; } = true;

    public bool EmitExplicitConversionToContainedTypeMethod { get; set; } = true;

    // IEquatable<TSelf>

    public bool EmitEqualsMethod { get; set; } = true;

    public bool EmitEqualsObjectMethod { get; set; } = true;

    public bool EmitGetHashCodeMethod { get; set; } = true;

    // IEqualityOperators<TSelf, TSelf, bool>,

    public bool EmitEqualityOperators { get; set; } = true;

    //  ISpanFormattable

    public bool EmitToStringFormatableMethod { get; set; } = true;

    public bool EmitToStringOverrideMethod { get; set; } = true;

    public bool EmitTryFormatMethod { get; set; } = true;

    //  ISpanParsable<T>

    public bool EmitTryParseSpanMethod { get; set; } = true;

    public bool EmitTryParseStringMethod { get; set; } = true;

    public bool EmitTryParseSpanNumberStylesMethod { get; set; }

    public bool EmitTryParseStringNumberStylesMethod { get; set; }

    public bool EmitParseSpanMethod { get; set; } = true;

    public bool EmitParseStringMethod { get; set; } = true;

    public bool EmitParseSpanNumberStylesMethod { get; set; }

    public bool EmitParseStringNumberStylesMethod { get; set; }

    public bool EmitUsingSystemGlobalization => EmitParseSpanNumberStylesMethod
        || EmitParseStringNumberStylesMethod
        || EmitTryParseSpanNumberStylesMethod
        || EmitTryParseStringNumberStylesMethod;
}
