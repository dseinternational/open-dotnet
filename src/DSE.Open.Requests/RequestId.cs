// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Requests;

/// <summary>
/// A request identifier: a non-empty string of up to
/// <see cref="MaxSerializedCharLength"/> characters. Stable across retries so the
/// server can deduplicate repeated deliveries of the same request.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<RequestId, CharSequence>))]
public readonly partial struct RequestId : IEquatableValue<RequestId, CharSequence>
{
    private const int Length = 200;

    /// <summary>The maximum length of a <see cref="RequestId"/> value.</summary>
    public static int MaxSerializedCharLength => Length;

    /// <summary>
    /// Initialises a new <see cref="RequestId"/> with the supplied string value.
    /// </summary>
    /// <param name="value">The identifier text. Must be non-empty and no longer than
    /// <see cref="MaxSerializedCharLength"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is empty or too long.</exception>
    public RequestId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"Value must be a non-whitespace string with no control characters and a maximum length of {Length} characters.");
        }

        _value = value;
    }

    /// <summary>
    /// Validates a candidate value: non-whitespace, no control characters, and no longer
    /// than <see cref="MaxSerializedCharLength"/>.
    /// </summary>
    public static bool IsValidValue(CharSequence value)
    {
        if (value.Length is 0 or > Length)
        {
            return false;
        }

        var hasNonWhiteSpace = false;

        foreach (var c in value.Span)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            hasNonWhiteSpace |= !char.IsWhiteSpace(c);
        }

        return hasNonWhiteSpace;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>Explicitly converts a string to a <see cref="RequestId"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is empty or too long.</exception>
    public static explicit operator RequestId(string value)
    {
        return new RequestId(value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _value.ToString();
    }
}
