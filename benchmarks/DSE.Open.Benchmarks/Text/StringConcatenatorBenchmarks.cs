// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using DSE.Open.Text;
using Microsoft.VisualBasic;

namespace DSE.Open.Benchmarks.Text;

[MemoryDiagnoser]
public class StringConcatenatorBenchmarks
{
    /*
    // use Hello World! as the string to join for easy
    private static readonly string[] s_short = ["Hello", "World", "!"];

    // use the ipsum text as the string to join for medium
    private static readonly string[] s_long =
    [
        "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Sed", "non", "risus.", "Suspendisse", "lectus", "tortor,",
        "dignissim", "sit", "amet,", "adipiscing", "nec,", "ultricies", "sed,", "dolor.", "Cras", "elementum", "ultrices", "diam.", "Maecenas", "ligula",
        "massa,", "varius", "a,", "semper", "congue,", "euismod", "non,", "mi.", "Proin", "porttitor,", "orci", "nec", "nonummy", "molestie,", "enim",
        "est", "eleifend", "mi,", "non", "fermentum", "diam", "nisi", "sit", "amet", "erat.", "Duis", "semper.", "Duis", "arcu", "massa,", "scelerisque",
        "vitae,", "consequat", "in,", "pretium", "a,", "enim.", "Pellentesque", "congue.", "Ut", "in", "risus", "voluptatem", "libero", "posuere",
        "consequat.", "Maecenas", "volutpat,", "diam", "enim", "sagittis", "quam,", "id", "consectetur", "mi", "nulla", "ac", "nibh.", "Fusce", "vulputate",
        "eleifend", "nulla.", "Cras", "ullamcorper", "consequat", "nisl.", "Maecenas", "nisl", "est,", "ultrices", "nec,", "gravida", "ac,", "vulputate",
        "vitae,", "nisl.", "Praesent", "viverra", "massa", "eget", "risus.", "Integer", "quis", "urna.", "Ut", "ante", "enim,", "dapibus", "ut,", "aliquam",
        "quis,", "sagittis", "non,"
    ];
    */
    // long but as a list
    private static readonly List<string> s_longList =
    [
        "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Sed", "non", "risus.", "Suspendisse", "lectus", "tortor,",
        "dignissim", "sit", "amet,", "adipiscing", "nec,", "ultricies", "sed,", "dolor.", "Cras", "elementum", "ultrices", "diam.", "Maecenas", "ligula",
        "massa,", "varius", "a,", "semper", "congue,", "euismod", "non,", "mi.", "Proin", "porttitor,", "orci", "nec", "nonummy", "molestie,", "enim",
        "est", "eleifend", "mi,", "non", "fermentum", "diam", "nisi", "sit", "amet", "erat.", "Duis", "semper.", "Duis", "arcu", "massa,", "scelerisque",
        "vitae,", "consequat", "in,", "pretium", "a,", "enim.", "Pellentesque", "congue.", "Ut", "in", "risus", "voluptatem", "libero", "posuere",
        "consequat.", "Maecenas", "volutpat,", "diam", "enim", "sagittis", "quam,", "id", "consectetur", "mi", "nulla", "ac", "nibh.", "Fusce", "vulputate",
        "eleifend", "nulla.", "Cras", "ullamcorper", "consequat", "nisl.", "Maecenas", "nisl", "est,", "ultrices", "nec,", "gravida", "ac,", "vulputate",
        "vitae,", "nisl.", "Praesent", "viverra", "massa", "eget", "risus.", "Integer", "quis", "urna.", "Ut", "ante", "enim,", "dapibus", "ut,", "aliquam",
        "quis,", "sagittis", "non,"
    ];
    // short but as a list

    //private static readonly List<string> s_shortList = ["Hello", "World", "!"];

    public static IEnumerable<object> Params()
    {
        //yield return s_short;
        //yield return s_long;
        yield return s_longList;
        //yield return s_shortList;
    }

    // Benchmark Join, Join_2, Join_3
    // with params Short, Long

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Params))]
    public string Join_Original(IEnumerable<string> values)
    {
        return StringConcatenator.Join_Original(", ", values, " and ");
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public string Join(IEnumerable<string> values)
    {
        return StringConcatenator.Join(", ", values, " and ");
    }

    //[Benchmark]
    //[ArgumentsSource(nameof(Params))]
    //public string Join2(IEnumerable<string> values)
    //{
    //    return StringConcatenator.Join2(", ", values, " and ");
    //}

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public string Join3(IEnumerable<string> values)
    {
        return StringConcatenator.Join3(", ", values, " and ");
    }
}
