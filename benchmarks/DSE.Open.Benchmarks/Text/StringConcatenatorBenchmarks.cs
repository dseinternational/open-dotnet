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
    // use Hello World! as the string to join for easy
    private Array Short = new[] { "Hello", "World", "!" };

    // use the ipsum text as the string to join for medium
    private Array Long = new[]
    {
        "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Sed", "non", "risus.", "Suspendisse", "lectus", "tortor,",
        "dignissim", "sit", "amet,", "adipiscing", "nec,", "ultricies", "sed,", "dolor.", "Cras", "elementum", "ultrices", "diam.", "Maecenas", "ligula",
        "massa,", "varius", "a,", "semper", "congue,", "euismod", "non,", "mi.", "Proin", "porttitor,", "orci", "nec", "nonummy", "molestie,", "enim",
        "est", "eleifend", "mi,", "non", "fermentum", "diam", "nisi", "sit", "amet", "erat.", "Duis", "semper.", "Duis", "arcu", "massa,", "scelerisque",
        "vitae,", "consequat", "in,", "pretium", "a,", "enim.", "Pellentesque", "congue.", "Ut", "in", "risus", "voluptatem", "libero", "posuere",
        "consequat.", "Maecenas", "volutpat,", "diam", "enim", "sagittis", "quam,", "id", "consectetur", "mi", "nulla", "ac", "nibh.", "Fusce", "vulputate",
        "eleifend", "nulla.", "Cras", "ullamcorper", "consequat", "nisl.", "Maecenas", "nisl", "est,", "ultrices", "nec,", "gravida", "ac,", "vulputate",
        "vitae,", "nisl.", "Praesent", "viverra", "massa", "eget", "risus.", "Integer", "quis", "urna.", "Ut", "ante", "enim,", "dapibus", "ut,", "aliquam",
        "quis,", "sagittis", "non,"
    };

    // long but as a list
    private List<string> LongList = new()
    {
        "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Sed", "non", "risus.", "Suspendisse", "lectus", "tortor,",
        "dignissim", "sit", "amet,", "adipiscing", "nec,", "ultricies", "sed,", "dolor.", "Cras", "elementum", "ultrices", "diam.", "Maecenas", "ligula",
        "massa,", "varius", "a,", "semper", "congue,", "euismod", "non,", "mi.", "Proin", "porttitor,", "orci", "nec", "nonummy", "molestie,", "enim",
        "est", "eleifend", "mi,", "non", "fermentum", "diam", "nisi", "sit", "amet", "erat.", "Duis", "semper.", "Duis", "arcu", "massa,", "scelerisque",
        "vitae,", "consequat", "in,", "pretium", "a,", "enim.", "Pellentesque", "congue.", "Ut", "in", "risus", "voluptatem", "libero", "posuere",
        "consequat.", "Maecenas", "volutpat,", "diam", "enim", "sagittis", "quam,", "id", "consectetur", "mi", "nulla", "ac", "nibh.", "Fusce", "vulputate",
        "eleifend", "nulla.", "Cras", "ullamcorper", "consequat", "nisl.", "Maecenas", "nisl", "est,", "ultrices", "nec,", "gravida", "ac,", "vulputate",
        "vitae,", "nisl.", "Praesent", "viverra", "massa", "eget", "risus.", "Integer", "quis", "urna.", "Ut", "ante", "enim,", "dapibus", "ut,", "aliquam",
        "quis,", "sagittis", "non,"
    };
    // short but as a list

    private List<string> ShortList = new() { "Hello", "World", "!" };


      public IEnumerable<object> Params()
    {
        yield return Short;
        yield return Long;
        yield return LongList;
        yield return ShortList;
    }


    // Benchmark Join, Join_2, Join_3
    // with params Short, Long

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Params))]
    public string Join_Original(string[] values)
    {
        return StringConcatenator.Join_Original(" ", values, " and ");
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public string Join(IEnumerable<string> values)
    {
        return StringConcatenator.Join(" ", values, " and ");
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public string Join2(IEnumerable<string> values)
    {
        return StringConcatenator.Join2(" ", values, " and ");
    }

}
