using App.Host.Controllers;
using App.Manager.Countries;
using App.Manager.Countries.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace App.Tests.Host;

[TestFixture]
public class CountryControllerTests
{
    [Test]
    public async Task GetAll_ShouldReturnOk()
    {
        var manager = new Mock<ICountryManager>();
        manager.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<CountryDTO>());

        var sut = new CountryController(manager.Object);

        var result = await sut.GetAll();

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Get_ShouldReturnNotFound_WhenManagerThrows()
    {
        var manager = new Mock<ICountryManager>();
        manager.Setup(x => x.GetAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var sut = new CountryController(manager.Object);

        var result = await sut.Get(1);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var manager = new Mock<ICountryManager>();
        manager.Setup(x => x.CreateAsync(It.IsAny<CreateCountryRequestDTO>()))
            .ReturnsAsync(new CountryDTO
            {
                Id = 1,
                Name = "Dominican Republic",
                Code = "DO",
                Coordinates = "18,-70",
                Cities = new List<App.Manager.Cities.DTO.CityDTO>()
            });

        var sut = new CountryController(manager.Object);

        var result = await sut.Create(new CreateCountryRequestDTO("Dominican Republic", "DO", 18, -70));

        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUrlIdDoesNotMatchBodyId()
    {
        var manager = new Mock<ICountryManager>();
        var sut = new CountryController(manager.Object);

        var result = await sut.Update(2, new UpdateCountryRequestDTO(1, "DR", "DO"));

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
}