// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// An exception that is throw when attempting to access a value that is unknown or is missing.
/// </summary>
/// <remarks>
/// <para>Examples include attempting to access the value of an <see cref="INullable"/>
/// where <see cref="INullable.HasValue"/> is <see langword="false"/>, or casting a
/// <see cref="Trilean"/> to a <see cref="bool"/> when <see cref="Trilean.IsUnknown"/> is
/// <see langword="true"/>.</para>
/// </remarks>
public class UnknownValueException : InvalidOperationException
{
    private const string DefaultMessage = "Cannot access value as the value is unknown.";

    public UnknownValueException() : base(DefaultMessage)
    {
    }

    public UnknownValueException(string message) : base(message ?? DefaultMessage)
    {
    }

    public UnknownValueException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    public static void ThrowIfNull(INullable value, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!value.HasValue)
        {
            throw new UnknownValueException(message!);
        }
    }

    public static void ThrowIfNull<T>(T? value, string? message = null)
    {
        if (value is null)
        {
            throw new UnknownValueException(message!);
        }
    }

    public static void ThrowIfUnknown(Trilean value, string? message = null)
    {
        if (value.IsUnknown)
        {
            throw new UnknownValueException(message!);
        }
    }
}
