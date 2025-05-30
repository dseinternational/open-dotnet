// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Numerics;

public partial class SequenceTests
{
    [Fact]
    public void SumInteger_Array_Int32()
    {
        int[] v = [1, 2, 3, 4, 5];
        var sum = Sequence.SumChecked(v);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void SumFloatingPoint_Array_Double()
    {
        double[] v = [1, 2, 3, 4, 5];
        var sum = Sequence.SumChecked(v);
        Assert.Equal(15.0, sum);
    }

    [Fact]
    public void SumFloatingPoint_Array_Double_with_NaN_returns_NaN()
    {
        double[] v = [1, 2, 3, 4, 5, double.NaN];
        var sum = Sequence.SumChecked(v);
        Assert.True(double.IsNaN(sum));
    }

    [Fact]
    public void SumInteger_Collection_Int32()
    {
        Collection<int> v = [1, 2, 3, 4, 5];
        var sum = Sequence.SumChecked(v);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void SumFloatingPoint_Collection_Double()
    {
        Collection<double> v = [1, 2, 3, 4, 5];
        var sum = Sequence.SumChecked(v);
        Assert.Equal(15.0, sum);
    }

    [Fact]
    public void SumFloatingPoint_Enumerable_Double_with_NaN_returns_NaN()
    {
        var sum = Sequence.SumChecked(GetSequence());
        Assert.True(double.IsNaN(sum));

        static IEnumerable<double> GetSequence()
        {
            foreach (var i in Enumerable.Range(1, 5))
            {
                yield return i;
            };
            yield return double.NaN;
        }
    }

    [Fact]
    public void SumFloatingPoint_Array_Double_Many()
    {
        var v = Enumerable.Range(1, 5000).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33)
            .ToArray();
        var sum = Sequence.SumChecked(v);
        Assert.Equal(3754504.5045045041, sum, 0.0000000005);
    }

    [Fact]
    public void SumFloatingPoint_Array_Double_Many_KahanBabushkaNeumaier()
    {
        var v = Enumerable.Range(1, 5000).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33)
            .ToArray();
        var sum = Sequence.SumChecked(v, SummationCompensation.KahanBabushkaNeumaier);
        // TODO: definitive test
        Assert.Equal(3754504.5045045046, sum, 0.0000000005);
    }
}
