// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// An exception that is throw when attempting to access a value that is unknown or is missing.
/// </summary>
/// <remarks>
/// <para>Examples include attempting to access the value of an <see cref="INaValue"/>
/// where <see cref="INaValue.HasValue"/> is <see langword="false"/>, or casting a
/// <see cref="Trilean"/> to a <see cref="bool"/> when <see cref="Trilean.IsNa"/> is
/// <see langword="true"/>.</para>
/// </remarks>
public class NaValueException : InvalidOperationException
{
    private const string DefaultMessage = "Cannot access value as the value is unknown.";

    public NaValueException() : base(DefaultMessage)
    {
    }

    public NaValueException(string message) : base(message ?? DefaultMessage)
    {
    }

    public NaValueException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    public static void ThrowIfNull(INaValue value, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!value.HasValue)
        {
            throw new NaValueException(message!);
        }
    }

    public static void ThrowIfNull<T>(T? value, string? message = null)
    {
        if (value is null)
        {
            throw new NaValueException(message!);
        }
    }

    public static void ThrowIfUnknown(Trilean value, string? message = null)
    {
        if (value.IsNa)
        {
            throw new NaValueException(message!);
        }
    }
}
