// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Geography;
using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class ValueConverterTests : SqliteInMemoryTestBase<TestDbContext>
{
    public override TestDbContext CreateContext(DbContextOptions<TestDbContext> contextOptions) => new(contextOptions);

    [Theory]
    [MemberData(nameof(CountryCodeStrings))]
    public void ConvertsToStoreType(CountryCode2 code, string expected)
    {
        var converter = new ValueConverter<CountryCode2, AsciiChar2, string>();
        var convertTo = converter.ConvertToProviderExpression.Compile();
        var result = convertTo(code);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(CountryCodeStrings))]
    public void ConvertsFromStoreType(CountryCode2 expected, string code)
    {
        var converter = new ValueConverter<CountryCode2, AsciiChar2, string>();
        var convertFrom = converter.ConvertFromProviderExpression.Compile();
        var result = convertFrom(code);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CanBeUsedInModel()
    {
        using var db = CreateContext();
        Assert.NotNull(db);
    }

    [Fact]
    public async Task ConvertedValueIsPersisted()
    {
        using (var db = CreateContext())
        {
            _ = db.Countries.Add(new Country { Code = CountryCode2.UnitedKingdom });
            _ = await db.SaveChangesAsync();
        }

        using (var db = CreateContext())
        {
            var c = await db.Countries.SingleAsync(c => c.Code == CountryCode2.UnitedKingdom);
            Assert.Equal(CountryCode2.UnitedKingdom, c.Code);
        }
    }

    public static TheoryData<CountryCode2, string> CountryCodeStrings
    {
        get
        {
            var data = new TheoryData<CountryCode2, string>();
            foreach (var countryCode in IsoCountryCodes.OfficiallyAssignedAlpha2)
            {
                data.Add(countryCode, countryCode.ToString());
            }

            return data;
        }
    }
}
