// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(Vector<bool>), "bool")]
[JsonDerivedType(typeof(Vector<char>), "char")]
[JsonDerivedType(typeof(Vector<string>), "string")]
[JsonDerivedType(typeof(NumericVector<byte>), "int8")]
[JsonDerivedType(typeof(NumericVector<short>), "int16")]
[JsonDerivedType(typeof(NumericVector<ushort>), "uint16")]
[JsonDerivedType(typeof(NumericVector<int>), "int32")]
[JsonDerivedType(typeof(NumericVector<uint>), "uint32")]
[JsonDerivedType(typeof(NumericVector<long>), "int64")]
[JsonDerivedType(typeof(NumericVector<uint>), "uint64")]
[JsonDerivedType(typeof(NumericVector<float>), "float32")]
[JsonDerivedType(typeof(NumericVector<double>), "float64")]
// [JsonDerivedType(typeof(NumericVector<Date64>), "date64")]
public abstract class Vector
{
    public static Vector Create<T>(T[] data)
        where T : notnull
    {
        return new Vector<T>(data);
    }

    public static Vector Create<T>(Memory<T> data)
        where T : notnull
    {
        return new Vector<T>(data);
    }

    public static Vector Create<T>(T[] data, int start, int length)
        where T : notnull
    {
        return new Vector<T>(data, start, length);
    }
}
