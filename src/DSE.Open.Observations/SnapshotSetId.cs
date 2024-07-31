// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Security;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value used to identify a <see cref="Snapshot"/>.
/// </summary>
[EquatableValue(AllowDefaultValue = false)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SnapshotSetId, ulong>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct SnapshotSetId
    : IEquatableValue<SnapshotSetId, ulong>,
      IUtf8SpanSerializable<SnapshotSetId>
{
    public const ulong MinIdValue = 1;
    public const ulong MaxIdValue = NumberHelper.MaxJsonSafeInteger;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public SnapshotSetId(ulong value) : this(value, false)
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

    public static bool TryFromInt64(long value, out SnapshotSetId id)
    {
        if (IsValidValue(value))
        {
            id = new SnapshotSetId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    public static SnapshotSetId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SnapshotSetId((ulong)value);
    }

    public static explicit operator SnapshotSetId(long value)
    {
        return FromInt64(value);
    }

    public static long ToInt64(SnapshotSetId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    public static implicit operator long(SnapshotSetId value)
    {
        return ToInt64(value);
    }

    public static SnapshotSetId GetRandomId()
    {
        return (SnapshotSetId)RandomValueGenerator.GetUInt64Value(MinIdValue, MaxIdValue);
    }
}
