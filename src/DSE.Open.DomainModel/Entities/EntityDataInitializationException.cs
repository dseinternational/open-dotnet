// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An exception that is thrown when backing data is not initialized as expected - for example, by
/// the ORM framework.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors",
    Justification = "ParameterName is required.")]
public class EntityDataInitializationException : EntityDataException
{
    private const string DefaultMessage = "An error occured initializing an entity with data " +
        "from the data store. Parameter name: ";

    /// <summary>
    /// Initializes a new <see cref="EntityDataInitializationException"/> with the
    /// supplied <paramref name="parameterName"/> and <paramref name="message"/>.
    /// </summary>
    public EntityDataInitializationException(string parameterName, string? message)
        : this(parameterName, null, message, null)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="EntityDataInitializationException"/>.
    /// </summary>
    /// <param name="parameterName">The name of the parameter or backing field that was not initialized.</param>
    /// <param name="validationResult">An optional validation result describing the failure.</param>
    /// <param name="message">An optional message; when not supplied a default message is generated.</param>
    /// <param name="innerException">An optional inner exception.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is <see langword="null"/>.</exception>
    public EntityDataInitializationException(string parameterName, ValidationResult? validationResult = null, string? message = null, Exception? innerException = null)
        : base(message ?? DefaultMessage + parameterName, innerException)
    {
        ArgumentNullException.ThrowIfNull(parameterName);

        ParameterName = parameterName;
        ValidationResult = validationResult;
    }

    /// <summary>
    /// The name of the parameter or backing field that was not initialized.
    /// </summary>
    public string ParameterName { get; }

    /// <summary>
    /// The validation result describing the failure, if one was supplied.
    /// </summary>
    public ValidationResult? ValidationResult { get; }

    /// <summary>
    /// Throws an <see cref="EntityDataInitializationException"/> if
    /// <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition to test.</param>
    /// <param name="parameterName">Automatically captured expression for <paramref name="condition"/>.</param>
    public static void ThrowIf(
        [DoesNotReturnIf(true)] bool condition,
        [CallerArgumentExpression("condition")] string? parameterName = null)
    {
        if (condition)
        {
            Throw(parameterName!);
        }
    }

    /// <summary>
    /// Throws an <see cref="EntityDataInitializationException"/> if <paramref name="entity"/>
    /// is null.
    /// </summary>
    /// <param name="entity">The <see cref="StoredObject"/> to validate as non-null.</param>
    /// <param name="parameterName">The name of the property with which <paramref name="entity"/>
    /// corresponds.</param>
    public static void ThrowIfNull([NotNull] StoredObject? entity, [CallerArgumentExpression("entity")] string? parameterName = null)
    {
        ThrowIf(entity is null, parameterName);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    internal static void Throw(string parameterName)
    {
        throw new EntityDataInitializationException(parameterName);
    }
}
