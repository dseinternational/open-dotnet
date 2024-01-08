// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Numerics;

public partial class SequenceTests
{
    [Fact]
    public void Sum_Array_Int32()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        var sum = Sequence.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Array_Double()
    {
        double[] sequence = [1, 2, 3, 4, 5];
        var sum = Sequence.Sum(sequence);
        Assert.Equal(15.0, sum);
    }

    [Fact]
    public void Sum_Array_Double_with_NaN_returns_NaN()
    {
        double[] sequence = [1, 2, 3, 4, 5, double.NaN];
        var sum = Sequence.Sum(sequence);
        Assert.True(double.IsNaN(sum));
    }

    [Fact]
    public void Sum_Collection_Int32()
    {
        Collection<int> sequence = [1, 2, 3, 4, 5];
        var sum = Sequence.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Collection_Double()
    {
        Collection<double> sequence = [1, 2, 3, 4, 5];
        var sum = Sequence.Sum(sequence);
        Assert.Equal(15.0, sum);
    }

    [Fact]
    public void Sum_Enumerable_Double_with_NaN_returns_NaN()
    {
        var sum = Sequence.Sum(GetSequence());
        Assert.True(double.IsNaN(sum));

        IEnumerable<double> GetSequence()
        {
            foreach (var i in Enumerable.Range(1,5))
            {
                yield return i;
            };
            yield return double.NaN;
        }
    }

    [Fact]
    public void Sum_Array_Double_Many()
    {
        var sequence = Enumerable.Range(1, 5000).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33)
            .ToArray();
        var sum = Sequence.Sum(sequence);
        Assert.Equal(3754504.5045045041, sum);
    }

    [Fact]
    public void Sum_Array_Double_Many_KahanBabushkaNeumaier()
    {
        var sequence = Enumerable.Range(1, 5000).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33)
            .ToArray();
        var sum = Sequence.Sum(sequence, SummationCompensation.KahanBabushkaNeumaier);
        Assert.Equal(3754504.5045045046, sum);
    }
}
