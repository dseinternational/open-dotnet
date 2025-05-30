// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class VectorCollectionExtensions
{
    // public static void Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
    //     this ICollection<Series> series,
    //     string name,
    //     T[] vector)
    //     where T : notnull
    // {
    //     ArgumentNullException.ThrowIfNull(series);
    //     series.Add(Series.Create(name, Vector.Create(vector)));
    // }
    //
    // public static void Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
    //     this ICollection<Series> series,
    //     string name,
    //     T[] vector,
    //     IDictionary<string, Variant>? annotations)
    //     where T : notnull
    // {
    //     ArgumentNullException.ThrowIfNull(series);
    //     series.Add(Series.Create(name, Vector.Create(vector), annotations));
    // }
    //
    // public static void AddNumeric<T>(this ICollection<Series> series, string name, T[] vector)
    //     where T : struct, INumber<T>
    // {
    //     ArgumentNullException.ThrowIfNull(series);
    //     series.Add(Series.CreateNumeric(name, Vector.CreateNumeric(vector)));
    // }
    //
    // public static void AddNumeric<T>(this ICollection<Series> series, string name, T[] vector, IDictionary<string, Variant>? annotations)
    //     where T : struct, INumber<T>
    // {
    //     ArgumentNullException.ThrowIfNull(series);
    //     series.Add(Series.CreateNumeric(name, Vector.CreateNumeric(vector), annotations));
    // }
}
