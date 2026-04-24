// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

public class DataSetTests
{
    [Theory]
    [MemberData(nameof(JsonOptions))]
    public void DataSet_serializes_and_deserializes_name_and_frames(JsonSerializerOptions options)
    {
        var dataSet = CreateDataSet();

        var json = JsonSerializer.Serialize(dataSet, options);
        var deserialized = JsonSerializer.Deserialize<DataSet>(json, options);

        Assert.NotNull(deserialized);
        Assert.Equal("dataset", deserialized.Name);
        Assert.Equal(2, deserialized.Count);
        AssertDataFrame(deserialized[0], "frame-1", [1, 2, 3]);
        AssertDataFrame(deserialized[1], "frame-2", [4, 5, 6]);
    }

    [Theory]
    [MemberData(nameof(JsonOptions))]
    public void ReadOnlyDataSet_serializes_and_deserializes_name_and_frames(JsonSerializerOptions options)
    {
        var dataSet = CreateDataSet().AsReadOnly();

        var json = JsonSerializer.Serialize(dataSet, options);
        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataSet>(json, options);

        Assert.NotNull(deserialized);
        Assert.Equal("dataset", deserialized.Name);
        Assert.Equal(2, deserialized.Count);
        AssertReadOnlyDataFrame(deserialized[0], "frame-1", [1, 2, 3]);
        AssertReadOnlyDataFrame(deserialized[1], "frame-2", [4, 5, 6]);
    }

    [Fact]
    public void AsReadOnly_preserves_name_and_frame_order()
    {
        var dataSet = CreateDataSet();

        var readOnly = dataSet.AsReadOnly();

        Assert.Equal("dataset", readOnly.Name);
        Assert.Equal(2, readOnly.Count);
        Assert.Equal("frame-1", readOnly[0].Name);
        Assert.Equal("frame-2", readOnly[1].Name);
    }

    [Fact]
    public void List_members_delegate_to_underlying_frame_collection()
    {
        var frame1 = CreateFrame("frame-1", [1, 2, 3]);
        var frame2 = CreateFrame("frame-2", [4, 5, 6]);
        var dataSet = new DataSet("dataset");

        dataSet.Add(frame1);
        dataSet.Insert(1, frame2);

        Assert.Equal(2, dataSet.Count);
        Assert.Same(frame1, dataSet[0]);
        Assert.Same(frame2, dataSet[1]);
        Assert.Contains(frame2, dataSet);
        Assert.Equal(1, dataSet.IndexOf(frame2));

        var copy = new DataFrame[2];
        dataSet.CopyTo(copy, 0);
        Assert.Same(frame1, copy[0]);
        Assert.Same(frame2, copy[1]);

        Assert.True(dataSet.Remove(frame1));
        Assert.Single(dataSet);

        dataSet.Clear();
        Assert.Empty(dataSet);
    }

    public static TheoryData<JsonSerializerOptions> JsonOptions => new()
    {
        NumericsJsonSharedOptions.Reflected,
        NumericsJsonSharedOptions.SourceGenerated,
    };

    private static DataSet CreateDataSet()
    {
        var dataSet = new DataSet("dataset")
        {
            CreateFrame("frame-1", [1, 2, 3]),
            CreateFrame("frame-2", [4, 5, 6]),
        };

        return dataSet;
    }

    private static DataFrame CreateFrame(string name, int[] values)
    {
        return new DataFrame(name)
        {
            Series.Create(values, "values"),
        };
    }

    private static void AssertDataFrame(DataFrame frame, string name, int[] expectedValues)
    {
        Assert.Equal(name, frame.Name);
        var series = Assert.IsType<Series<int>>(frame["values"]);
        Assert.True(series.AsSpan().SequenceEqual(expectedValues));
    }

    private static void AssertReadOnlyDataFrame(ReadOnlyDataFrame frame, string name, int[] expectedValues)
    {
        Assert.Equal(name, frame.Name);
        var series = Assert.IsType<ReadOnlySeries<int>>(frame["values"]);
        Assert.True(series.Vector.AsSpan().SequenceEqual(expectedValues));
    }
}
