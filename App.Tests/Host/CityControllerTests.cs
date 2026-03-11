using App.Host.Controllers;
using App.Manager.Cities;
using App.Manager.Cities.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace App.Tests.Host;

[TestFixture]
public class CityControllerTests
{
    [Test]
    public async Task GetAll_ShouldReturnOk()
    {
        var manager = new Mock<ICityManager>();
        manager.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<CityDTO>());

        var sut = new CityController(manager.Object);

        var result = await sut.GetAll();

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Get_ShouldReturnNotFound_WhenManagerThrows()
    {
        var manager = new Mock<ICityManager>();
        manager.Setup(x => x.GetAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var sut = new CityController(manager.Object);

        var result = await sut.Get(1);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var manager = new Mock<ICityManager>();
        manager.Setup(x => x.CreateAsync(It.IsAny<CreateCityRequestDTO>()))
            .ReturnsAsync(new CityDTO
            {
                Id = 1,
                Name = "Santo Domingo",
                CountryId = 1
            });

        var sut = new CityController(manager.Object);

        var result = await sut.Create(new CreateCityRequestDTO("Santo Domingo", 1));

        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUrlIdDoesNotMatchBodyId()
    {
        var manager = new Mock<ICityManager>();
        var sut = new CityController(manager.Object);

        var result = await sut.Update(2, new UpdateCityRequestDTO(1, "Santiago", 1));

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
}