using App.Access.Countries;
using App.Access.Entities;
using App.Engine.Countries;
using App.Manager.Countries;
using App.Manager.Countries.DTO;
using Moq;
using NUnit.Framework;

namespace App.Tests.Manager;

[TestFixture]
public class CountryManagerTests
{
    [Test]
    public async Task CreateAsync_ShouldValidate_Add_AndReturnDto()
    {
        var validation = new Mock<ICountryValidationEngine>();
        var access = new Mock<ICountryAccess>();

        access.Setup(x => x.AddAsync(It.IsAny<Country>()))
            .ReturnsAsync((Country c) =>
            {
                c.Id = 7;
                return c;
            });

        var sut = new CountryManager(validation.Object, access.Object);

        var result = await sut.CreateAsync(new CreateCountryRequestDTO(
            "Dominican Republic",
            "DO",
            18,
            -70));

        validation.Verify(x => x.ValidateCreateRequestAsync("Dominican Republic", "DO", 18, -70), Times.Once);
        access.Verify(x => x.AddAsync(It.IsAny<Country>()), Times.Once);

        Assert.That(result.Id, Is.EqualTo(7));
        Assert.That(result.Name, Is.EqualTo("Dominican Republic"));
        Assert.That(result.Code, Is.EqualTo("DO"));
        Assert.That(result.Coordinates, Is.EqualTo("18,-70"));
    }

    [Test]
    public async Task GetAsync_ShouldThrow_WhenCountryDoesNotExist()
    {
        var validation = new Mock<ICountryValidationEngine>();
        var access = new Mock<ICountryAccess>();
        access.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Country?)null);

        var sut = new CountryManager(validation.Object, access.Object);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await sut.GetAsync(99));
    }

    [Test]
    public async Task GetAllAsync_ShouldMapEntitiesToDtos()
    {
        var validation = new Mock<ICountryValidationEngine>();
        var access = new Mock<ICountryAccess>();

        access.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Country>
        {
            new()
            {
                Id = 1,
                CountryName = "Dominican Republic",
                CountryCode = "DO",
                Coordinates = "18,-70",
                Cities = new List<City>()
            }
        });

        var sut = new CountryManager(validation.Object, access.Object);

        var result = await sut.GetAllAsync();

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Dominican Republic"));
    }

    [Test]
    public async Task UpdateAsync_ShouldValidate_AndReturnMappedDto()
    {
        var validation = new Mock<ICountryValidationEngine>();
        var access = new Mock<ICountryAccess>();

        access.Setup(x => x.UpdateAsync(It.IsAny<Country>()))
            .ReturnsAsync((Country c) =>
            {
                c.Coordinates = "18,-70";
                c.Cities = new List<City>();
                return c;
            });

        var sut = new CountryManager(validation.Object, access.Object);

        var result = await sut.UpdateAsync(new UpdateCountryRequestDTO(1, "República Dominicana", "DOM"));

        validation.Verify(x => x.ValidateUpdateRequestAsync(1, "República Dominicana", "DOM"), Times.Once);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("República Dominicana"));
        Assert.That(result.Code, Is.EqualTo("DOM"));
    }

    [Test]
    public async Task DeleteAsync_ShouldDelegateToAccess()
    {
        var validation = new Mock<ICountryValidationEngine>();
        var access = new Mock<ICountryAccess>();

        var sut = new CountryManager(validation.Object, access.Object);

        await sut.DeleteAsync(5);

        access.Verify(x => x.DeleteAsync(5), Times.Once);
    }
}