// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;

namespace DSE.Open.Tests.Time;

public class StandardTimeZoneInfoTests
{
    [Fact]
    public void GetAll_and_get_system_info()
    {
        var timeZones = StandardTimeZoneInfo.GetAll();

        Assert.NotNull(timeZones);

        foreach (var tz in timeZones)
        {
            var systemInfo = tz.GetTimeZoneInfo();
            Assert.NotNull(systemInfo);
        }
    }
}
