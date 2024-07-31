// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Security;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value used to identify a <see cref="ObservationSnapshot"/>.
/// </summary>
[EquatableValue(AllowDefaultValue = false)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<ObservationSnapshotSetId, ulong>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct ObservationSnapshotSetId
    : IEquatableValue<ObservationSnapshotSetId, ulong>,
      IUtf8SpanSerializable<ObservationSnapshotSetId>
{
    public const ulong MinIdValue = 1;
    public const ulong MaxIdValue = NumberHelper.MaxJsonSafeInteger;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public ObservationSnapshotSetId(ulong value) : this(value, false)
    {
    }

    public static bool IsValidValue(ulong value)
    {
        return value is <= MaxIdValue and >= MinIdValue;
    }

    public static bool IsValidValue(long value)
    {
        return value is <= ((long)MaxIdValue) and >= ((long)MinIdValue);
    }

    public static bool TryFromInt64(long value, out ObservationSnapshotSetId id)
    {
        if (IsValidValue(value))
        {
            id = new ObservationSnapshotSetId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    public static ObservationSnapshotSetId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new ObservationSnapshotSetId((ulong)value);
    }

    public static explicit operator ObservationSnapshotSetId(long value)
    {
        return FromInt64(value);
    }

    public static long ToInt64(ObservationSnapshotSetId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    public static implicit operator long(ObservationSnapshotSetId value)
    {
        return ToInt64(value);
    }

    public static ObservationSnapshotSetId GetRandomId()
    {
        return (ObservationSnapshotSetId)RandomValueGenerator.GetUInt64Value(MinIdValue, MaxIdValue);
    }
}
