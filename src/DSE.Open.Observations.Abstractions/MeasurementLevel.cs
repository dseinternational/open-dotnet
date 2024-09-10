// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<MeasurementLevel, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct MeasurementLevel
    : IEquatableValue<MeasurementLevel, AsciiString>,
      IUtf8SpanSerializable<MeasurementLevel>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => MaxSerializedCharLength;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length is > 1 and <= 17 && Lookup.ContainsKey(value);
    }

    /// <summary>
    /// Measures one of only two possible outcomes or attributes.
    /// </summary>
    public static readonly MeasurementLevel Binary = new((AsciiString)"binary", true);

    /// <summary>
    /// Measures if something is in a category where the categories do not
    /// have any order or priority - for example, gender or blood type.
    /// </summary>
    public static readonly MeasurementLevel Nominal = new((AsciiString)"nominal", true);

    /// <summary>
    /// Measures if something is in a category where the categories do not
    /// have any order or priority, but do express a degree of membership or
    /// distance from prototype.
    /// </summary>
    public static readonly MeasurementLevel GradedMembership = new((AsciiString)"graded_membership", true);

    /// <summary>
    /// Measures if something is in a category where the categories do have an
    /// order, but the differences between categories are not equal - for example
    /// Likert scales, education level and socio-economic status.
    /// </summary>
    public static readonly MeasurementLevel Ordinal = new((AsciiString)"ordinal", true);

    /// <summary>
    /// Interval level data is numerical and the difference between
    /// two values is meaningful. The zero point is arbitrary and does
    /// not represent an absence of the characteristic. For example,
    /// temperature measured in Celsius or Fahrenheit.
    /// </summary>
    public static readonly MeasurementLevel Interval = new((AsciiString)"interval", true);

    /// <summary>
    /// Interval level data on a logarithmic scale. Can be rescaled by changing
    /// the exponent (the zero remains fixed).
    /// </summary>
    public static readonly MeasurementLevel LogInterval = new((AsciiString)"log_interval", true);

    /// <summary>
    /// Ratio level data is numerical, but the zero point is not arbitrary.
    /// It indicates an absence of the characteristic being measured.
    /// Examples include weight, height, and age.
    /// Unit of measure (additive rule applies)
    /// </summary>
    public static readonly MeasurementLevel ExtensiveRatio = new((AsciiString)"extensive_ratio", true);

    /// <summary>
    /// Interval level data on a cyclical scale - for example, angles (where the
    /// direction 359° is as far from 0° as 1° is).
    /// </summary>
    /// <remarks>Definition requires unit of measure plus length of cycle.</remarks>
    public static readonly MeasurementLevel CyclicRatio = new((AsciiString)"cyclic_ratio", true);

    /// <summary>
    /// Units of measures (formula of combination)
    /// </summary>
    public static readonly MeasurementLevel DerivedRatio = new((AsciiString)"derived_ratio", true);

    /// <summary>
    /// A count of something (non-negative integer).
    /// </summary>
    public static readonly MeasurementLevel Count = new((AsciiString)"count", true);

    /// <summary>
    /// An amount of something (non-negative real number).
    /// </summary>
    public static readonly MeasurementLevel Amount = new((AsciiString)"amount", true);

    /// <summary>
    /// Ratio level (numerical) data where the value of 'one' (the unit of measurement)
    /// is fixed. The whole scale is therefore predetermined or 'absolute'. For example,
    /// a probability, the axioms of which fix the meaning of zero and one.
    /// </summary>
    public static readonly MeasurementLevel Absolute = new((AsciiString)"absolute", true);

    public static readonly IReadOnlyCollection<MeasurementLevel> All =
    [
        Binary,
        Nominal,
        GradedMembership,
        Ordinal,
        Interval,
        LogInterval,
        ExtensiveRatio,
        CyclicRatio,
        DerivedRatio,
        Count,
        Amount,
        Absolute
    ];

    public static readonly IReadOnlyDictionary<AsciiString, MeasurementLevel> Lookup = All.ToDictionary(r => r._value);
}
