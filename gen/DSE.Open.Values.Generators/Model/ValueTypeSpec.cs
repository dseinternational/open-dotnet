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

    public IEnumerable<string> Fields { get; set; } = [];

    public string ContainedValueTypeName { get; set; } = default!;

    public string ImplementedInterface => ValueTypeKind switch
    {
        ValueTypeKind.Equatable => $"{TypeNames.IEquatableValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Comparable => $"{TypeNames.IComparableValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Addable => $"{TypeNames.AddableValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        ValueTypeKind.Divisible => $"{TypeNames.IDivisibleValueInterfaceName}<{ValueTypeName}, {ContainedValueTypeName}>",
        _ => throw new NotSupportedException(),
    };

    public bool EmitUtf8SpanSerializableInterface { get; set; }

    public bool EmitValueField { get; set; } = true;

    public string ValueFieldName { get; set; } = "_value";

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

    public Accessibility ConstructorAccessibility { get; set; } = Accessibility.Private;

    public string ConstructorAccessibilityValue => AccessibilityHelper.GetKeyword(ConstructorAccessibility);

    public bool EmitMaxSerializedCharLength => MaxSerializedCharLength > 0;

    public int MaxSerializedCharLength { get; set; }

    // ITryConvertibleFrom<TSelf, T>

    public bool EmitEnsureIsValidValueMethod { get; set; } = true;

    public bool EmitTryFromValueMethod { get; set; } = true;

    public bool EmitFromValueMethod { get; set; } = true;

    public bool EmitExplicitConversionFromContainedTypeMethod { get; set; } = true;

    // IConvertibleTo<TSelf, T>

    public bool EmitConvertToMethod { get; set; } = true;

    public bool EmitExplicitConversionToContainedType { get; set; } = true;

    public bool EmitImplicitConversionToContainedTypeMethod { get; set; } = true;

    // IEquatable<TSelf>

    public bool EmitEqualsMethod { get; set; } = true;

    public bool EmitEqualsObjectMethod { get; set; } = true;

    public bool EmitGetHashCodeMethod { get; set; } = true;

    // IEqualityOperators<TSelf, TSelf, bool>,

    public bool EmitEqualityOperators { get; set; } = true;

    //  ISpanFormattable

    public bool EmitToStringFormattableMethod { get; set; } = true;

    public bool EmitToStringOverrideMethod { get; set; } = true;

    public bool EmitTryFormatMethod { get; set; } = true;

    //  ISpanParsable<T>
    public bool EmitParseSpanMethod { get; set; }

    public bool EmitTryParseSpanMethod { get; set; }

    public bool EmitParseSpanNumberStylesMethod { get; set; }

    public bool EmitTryParseSpanNumberStylesMethod { get; set; }

    // IParsable<TSelf>
    public bool EmitParseStringMethod { get; set; }

    public bool EmitTryParseStringMethod { get; set; }

    public bool EmitParseStringNumberStylesMethod { get; set; }

    public bool EmitTryParseStringNumberStylesMethod { get; set; }

    // IUtf8SpanFormattable

    public bool EmitTryFormatUtf8Method { get; set; }

    // IUtf8SpanParsable<T>

    public bool EmitParseUtf8Method { get; set; }

    public bool EmitTryParseUtf8Method { get; set; }
}
