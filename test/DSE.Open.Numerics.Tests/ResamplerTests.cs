// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public sealed class ResamplerTests
{
    [Fact]
    public void Resample_WithEmpty_ShouldReturnEmpty()
    {
        var frequency = ResamplingFrequency.Daily;
        var method = ResamplingMethod.Mean;

        var result = Resampler.Resample<int>([], [], frequency, method);

        Assert.All(result, s => Assert.True(s.IsEmpty));
    }

    [Fact]
    public void Resample_WithSingleValue_ShouldReturnSingleValue()
    {
        var frequency = ResamplingFrequency.Daily;
        var method = ResamplingMethod.Mean;

        int[] values = [1];
        DateTimeOffset[] dates = [new DateTimeOffset(2023, 10, 1, 0, 0, 0, TimeSpan.Zero)];

        var result = Resampler.Resample(dates, values, frequency, method);

        var period = result["period"] as ReadOnlySeries<DateTimeOffset>;
        var sample = result["value"] as ReadOnlySeries<int>;

        Assert.Equal(1, period?.Length);
        Assert.Equal(1, sample?.Length);

        Assert.Equal(dates[0], period![0]);
        Assert.Equal(values[0], sample![0]);
    }

    [Fact]
    public void Resample_Daily()
    {
        var frequency = ResamplingFrequency.Daily;
        var method = ResamplingMethod.Mean;

        int[] values = [1, 2, 3, 4, 5];
        DateTimeOffset[] dates = [
            new DateTimeOffset(2023, 10, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2023, 10, 2, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2023, 10, 2, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2023, 10, 3, 0, 0, 0, TimeSpan.Zero)
        ];

        var result = Resampler.Resample(dates, values, frequency, method);

        var period = result["period"] as ReadOnlySeries<DateTimeOffset>;
        var sample = result["value"] as ReadOnlySeries<int>;

        Assert.Equal(3, period?.Length);
        Assert.Equal(3, sample?.Length);

        Assert.Equal(new DateTimeOffset(2023, 10, 1, 0, 0, 0, TimeSpan.Zero), period![0]);
        Assert.Equal(1, sample![0]); // Mean of 1 and 2
        Assert.Equal(new DateTimeOffset(2023, 10, 2, 0, 0, 0, TimeSpan.Zero), period![1]);
        Assert.Equal(3, sample![1]); // Mean of 3 and 4
        Assert.Equal(new DateTimeOffset(2023, 10, 3, 0, 0, 0, TimeSpan.Zero), period![2]);
        Assert.Equal(5, sample![2]); // Single value for the third day
    }
}
