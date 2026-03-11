using App.Access.Countries;
using App.Access.Entities;
using App.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace App.Tests.Access;

[TestFixture]
public class CountryAccessTests
{
    [Test]
    public async Task AddAsync_ShouldPersistCountry()
    {
        await using var db = TestDbContextFactory.Create("test");
        var sut = new CountryAccess(db);

        var result = await sut.AddAsync(new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        });

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(await db.Countries.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCountryWithCities()
    {
        await using var db = TestDbContextFactory.Create();

        var country = new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70",
            Cities = new List<City>()
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();

        db.Cities.Add(new City
        {
            Name = "Santo Domingo",
            CountryId = country.Id
        });

        await db.SaveChangesAsync();

        var sut = new CountryAccess(db);
        var result = await sut.GetByIdAsync(country.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Cities, Is.Not.Null);
        Assert.That(result.Cities!.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateAsync_ShouldPatchOnlyNonNullProperties()
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

        db.ChangeTracker.Clear();

        var sut = new CountryAccess(db);

        var updated = await sut.UpdateAsync(new Country
        {
            Id = country.Id,
            CountryName = "República Dominicana",
            CountryCode = "DOM",
            Coordinates = null!
        });

        Assert.That(updated.CountryName, Is.EqualTo("República Dominicana"));
        Assert.That(updated.CountryCode, Is.EqualTo("DOM"));

        var persisted = await db.Countries.AsNoTracking().FirstAsync(x => x.Id == country.Id);
        Assert.That(persisted.Coordinates, Is.EqualTo("18,-70"));
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCountryExists()
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

        var sut = new CountryAccess(db);

        var exists = await sut.ExistsAsync(country.Id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task IsCodeUniqueAsync_ShouldReturnFalse_WhenCodeAlreadyExists()
    {
        await using var db = TestDbContextFactory.Create();
        db.Countries.Add(new Country
        {
            CountryName = "Dominican Republic",
            CountryCode = "DO",
            Coordinates = "18,-70"
        });
        await db.SaveChangesAsync();

        var sut = new CountryAccess(db);

        var result = await sut.IsCodeUniqueAsync("DO");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveCountry()
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

        var sut = new CountryAccess(db);
        await sut.DeleteAsync(country.Id);

        Assert.That(await db.Countries.CountAsync(), Is.EqualTo(0));
    }
}