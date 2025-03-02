// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(VectorJsonConverter))]
public abstract class Vector : IVector
{
    protected Vector(VectorDataType dataType, Type itemType, int length)
    {
        ArgumentNullException.ThrowIfNull(itemType);

#if DEBUG
        if (VectorDataTypeHelper.TryGetVectorDataType(itemType, out var expectedDataType)
            && dataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for " +
                $"item type {itemType.Name} but given {dataType}.");
        }
#endif

        DataType = dataType;

        IsNumeric = NumberHelper.IsKnownNumberType(itemType);

        ItemType = itemType;

        Length = length;
    }

    /// <summary>
    /// Gets the number of items in the vector.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric { get; }

    /// <summary>
    /// Gets the type of the items in the vector.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Gets the data type of the vector.
    /// </summary>
    public VectorDataType DataType { get; }

    [RequiresDynamicCode("Calls System.Type.MakeGenericType(params Type[])")]
    private static bool TryCreateNumericVector(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dataType,
        object data,
        [NotNullWhen(true)] out Vector? vector)
    {
        var constructedType = typeof(NumericVector<>).MakeGenericType(dataType);

        object[] args = [data];

        vector = Activator.CreateInstance(
            constructedType,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            args,
            null) as Vector;

        return vector is not null;
    }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "Type T will not be trimmed and the use of NumericVector<> can be statically determined.")]
    public static Vector<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (NumberHelper.IsKnownNumberType(typeof(T)))
        {
            if (TryCreateNumericVector(typeof(T), data, out var vector))
            {
                return (Vector<T>)vector;
            }
        }

        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new Vector<T>(data);
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "Type T will not be trimmed and the use of NumericVector<> can be statically determined.")]
    public static Vector<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(Memory<T> data)
    {
        if (NumberHelper.IsKnownNumberType(typeof(T)))
        {
            if (TryCreateNumericVector(typeof(T), data, out var vector))
            {
                return (Vector<T>)vector;
            }
        }

        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new Vector<T>(data);
    }

    public static Vector<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(ReadOnlySpan<T> data)
    {
        return Create(data.ToArray());
    }

    public static Vector<T> Create<T>(T[] data, int start, int length)
    {
        EnsureNotKnownNumericType(typeof(T));
        return new Vector<T>(data, start, length);
    }

    public static NumericVector<T> CreateNumeric<T>(T[] data, bool copy = false)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(copy ? [.. data] : data);
    }

    public static NumericVector<T> CreateNumeric<T>(Memory<T> data, bool copy = false)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(copy ? data.ToArray() : data);
    }

    public static NumericVector<T> CreateNumeric<T>(ReadOnlySpan<T> data)
        where T : struct, INumber<T>
    {
        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return NumericVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new NumericVector<T>(data.ToArray());
    }

    public static NumericVector<T> CreateNumeric<T>(T[] data, int start, int length)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(data, start, length);
    }

    public static NumericVector<T> CreateNumeric<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static NumericVector<T> CreateNumeric<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static NumericVector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return CreateNumeric(length, T.Zero);
    }

    public static NumericVector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return CreateNumeric(length, T.One);
    }

    /// <summary>
    /// Creates a categorical vector from the given data and categories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="categories"></param>
    /// <param name="copyData"></param>
    /// <param name="copyCatgories"></param>
    /// <returns></returns>
    /// <remarks>
    /// The category labels are not validated beyond checking that there are not more labels than
    /// values (the length of <paramref name="data"/>). To check if there is a label for each unique
    /// value in <paramref name="data"/>, call <see cref="CategoricalVector{T}.IsValid"/>.
    /// </remarks>
    public static CategoricalVector<T> CreateCategorical<T>(
        T[] data,
        Memory<KeyValuePair<string, T>> categories,
        bool copyData = false,
        bool copyCatgories = false)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        return new CategoricalVector<T>(copyData ? [.. data] : data, copyCatgories ? categories.ToArray() : categories);
    }

    /// <summary>
    /// Creates a categorical vector from the given data and categories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="categories"></param>
    /// <param name="copyData"></param>
    /// <param name="copyCatgories"></param>
    /// <returns></returns>
    /// <remarks>
    /// The category labels are not validated beyond checking that there are not more labels than
    /// values (the length of <paramref name="data"/>). To check if there is a label for each unique
    /// value in <paramref name="data"/>, call <see cref="CategoricalVector{T}.IsValid"/>.
    /// </remarks>
    public static CategoricalVector<T> CreateCategorical<T>(
        Memory<T> data,
        Memory<KeyValuePair<string, T>> categories,
        bool copyData = false,
        bool copyCatgories = false)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        return new CategoricalVector<T>(copyData ? data.ToArray() : data, copyCatgories ? categories.ToArray() : categories);
    }

    /// <summary>
    /// Creates a categorical vector from the given data and categories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="categories"></param>
    /// <returns></returns>
    /// <remarks>
    /// The category labels are not validated beyond checking that there are not more labels than
    /// values (the length of <paramref name="data"/>). To check if there is a label for each unique
    /// value in <paramref name="data"/>, call <see cref="CategoricalVector{T}.IsValid"/>.
    /// </remarks>
    public static CategoricalVector<T> CreateCategorical<T>(
        ReadOnlySpan<T> data,
        ReadOnlySpan<KeyValuePair<string, T>> categories)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return CategoricalVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new CategoricalVector<T>(data.ToArray(), categories.ToArray());
    }

    internal static void EnsureNotKnownNumericType(Type type)
    {
        if (NumberHelper.IsKnownNumberType(type))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Expected non-numeric type but {type.Name} is numeric.");
        }
    }
}
