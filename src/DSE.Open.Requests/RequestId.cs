// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Requests;

[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<RequestId, CharSequence>))]
public readonly partial struct RequestId : IEquatableValue<RequestId, CharSequence>
{
    private const int Length = 200;

    public static int MaxSerializedCharLength => Length;

    public RequestId(string value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"Value must be a string with a maximum length of {Length} characters.");
        }

        _value = value;
    }

    public static bool IsValidValue(CharSequence value)
    {
        return value.Length <= Length;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator RequestId(string value)
    {
        return new RequestId(value);
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}
