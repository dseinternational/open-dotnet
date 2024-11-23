// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// A value that is discriminated by another value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TDiscriminator"></typeparam>
public readonly record struct Discriminated<TValue, TDiscriminator>
    : IEquatable<Discriminated<TValue, TDiscriminator>>
    where TValue : struct, IEquatable<TValue>
    where TDiscriminator : IEquatable<TDiscriminator>
{
    public static readonly Discriminated<TValue, TDiscriminator> Empty;

    /// <summary>
    /// Initializes a new value with the given value and discriminator.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="discriminator"></param>
    [JsonConstructor]
    public Discriminated(TValue value, TDiscriminator discriminator)
    {
        Value = value;
        Discriminator = discriminator;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    [JsonPropertyName("v")]
    public TValue Value { get; }

    /// <summary>
    /// Gets the discriminator for the value.
    /// </summary>
    [JsonPropertyName("d")]
    public TDiscriminator Discriminator { get; }

    public void Deconstruct(out TValue Value, out TDiscriminator Discriminator)
    {
        Value = this.Value;
        Discriminator = this.Discriminator;
    }
}

/// <summary>
/// Helpers for creating discriminated values.
/// </summary>
public static class Discriminated
{
    /// <summary>
    /// Creates a discrimated value from the specified value and discriminator.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TDiscriminator">The type of the discriminator.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="discriminator">The discriminator.</param>
    /// <returns>A discriminated value combining the specified value and discriminator.</returns>
    public static Discriminated<TValue, TDiscriminator> Create<TValue, TDiscriminator>(
        TValue value,
        TDiscriminator discriminator)
        where TValue : struct, IEquatable<TValue>
        where TDiscriminator : IEquatable<TDiscriminator>
    {
        return new Discriminated<TValue, TDiscriminator>(value, discriminator);
    }
}
