// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TimestampedExtensionsTests
{
    [Fact]
    public void IsPersisted_NoTimestamp_ReturnsFalse()
    {
        var entity = new TimestampedFake(timestamp: null);

        Assert.False(entity.IsPersisted());
    }

    [Fact]
    public void IsPersisted_WithTimestamp_ReturnsTrue()
    {
        var entity = new TimestampedFake(new Timestamp([1, 2, 3, 4, 5, 6, 7, 8]));

        Assert.True(entity.IsPersisted());
    }

    [Fact]
    public void IsPersisted_NullSource_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            ((ITimestamped)null!).IsPersisted());
    }

    private sealed class TimestampedFake : ITimestamped
    {
        public TimestampedFake(Timestamp? timestamp)
        {
            Timestamp = timestamp;
        }

        public Timestamp? Timestamp { get; }
    }
}
