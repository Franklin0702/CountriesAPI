using App.Access.Cities;
using App.Access.Entities;
using App.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace App.Tests.Access;

[TestFixture]
public class CityAccessTests
{
    [Test]
    public async Task AddAsync_ShouldPersistCityAndLoadCountry()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var sut = new CityAccess(db);

        var result = await sut.AddAsync(new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        });

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Country, Is.Not.Null);
        Assert.That(result.Country!.CountryCode, Is.EqualTo("DO"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCityWithCountry()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var city = new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        };

        db.Cities.Add(city);
        await db.SaveChangesAsync();

        var sut = new CityAccess(db);
        var result = await sut.GetByIdAsync(city.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Country, Is.Not.Null);
        Assert.That(result.Country!.Id, Is.EqualTo(country.Id));
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateCity()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var city = new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        };

        db.Cities.Add(city);
        await db.SaveChangesAsync();

        var sut = new CityAccess(db);

        var updated = await sut.UpdateAsync(new City
        {
            Id = city.Id,
            Name = "Santiago",
            CountryId = country.Id
        });

        Assert.That(updated.Name, Is.EqualTo("Santiago"));
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCityExists()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var city = new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        };

        db.Cities.Add(city);
        await db.SaveChangesAsync();

        var sut = new CityAccess(db);

        var result = await sut.ExistsAsync(city.Id);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveCity()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var city = new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        };

        db.Cities.Add(city);
        await db.SaveChangesAsync();

        var sut = new CityAccess(db);
        await sut.DeleteAsync(city.Id);

        Assert.That(await db.Cities.CountAsync(), Is.EqualTo(0));
    }
}