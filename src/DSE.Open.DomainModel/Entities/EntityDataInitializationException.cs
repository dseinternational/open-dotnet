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

    public EntityDataInitializationException(string parameterName, string? message)
        : this(parameterName, null, message, null)
    {
    }

    public EntityDataInitializationException(string parameterName, ValidationResult? validationResult, string? message = null)
        : this(parameterName, validationResult, message, null)
    {
    }

    public EntityDataInitializationException(string parameterName, ValidationResult? validationResult = null, string? message = null, Exception? innerException = null)
        : base(message ?? DefaultMessage + parameterName, innerException)
    {
        Guard.IsNotNull(parameterName);

        ParameterName = parameterName;
        ValidationResult = validationResult;
    }

    public string ParameterName { get; }

    public ValidationResult? ValidationResult { get; }

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
