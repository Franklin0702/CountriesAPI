using App.Access.Cities;
using App.Access.Entities;
using App.Engine.Cities;
using App.Manager.Cities;
using App.Manager.Cities.DTO;
using Moq;
using NUnit.Framework;

namespace App.Tests.Manager;

[TestFixture]
public class CityManagerTests
{
    [Test]
    public async Task CreateAsync_ShouldValidate_Add_AndReturnDto()
    {
        var validation = new Mock<ICityValidationEngine>();
        var access = new Mock<ICityAccess>();

        access.Setup(x => x.AddAsync(It.IsAny<City>()))
            .ReturnsAsync((City c) =>
            {
                c.Id = 3;
                return c;
            });

        ICityManager sut = new CityManager(validation.Object, access.Object);

        var result = await sut.CreateAsync(new CreateCityRequestDTO("Santo Domingo", 1));

        validation.Verify(x => x.ValidateCreateRequestAsync("Santo Domingo", 1), Times.Once);
        access.Verify(x => x.AddAsync(It.IsAny<City>()), Times.Once);

        Assert.That(result.Id, Is.EqualTo(3));
        Assert.That(result.Name, Is.EqualTo("Santo Domingo"));
        Assert.That(result.CountryId, Is.EqualTo(1));
    }

    [Test]
    public async Task GetAsync_ShouldThrow_WhenCityDoesNotExist()
    {
        var validation = new Mock<ICityValidationEngine>();
        var access = new Mock<ICityAccess>();
        access.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((City?)null);

        ICityManager sut = new CityManager(validation.Object, access.Object);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await sut.GetAsync(99));
    }

    [Test]
    public async Task UpdateAsync_ShouldValidate_AndReturnDto()
    {
        var validation = new Mock<ICityValidationEngine>();
        var access = new Mock<ICityAccess>();

        access.Setup(x => x.UpdateAsync(It.IsAny<City>()))
            .ReturnsAsync((City c) => c);

        ICityManager sut = new CityManager(validation.Object, access.Object);

        var result = await sut.UpdateAsync(new UpdateCityRequestDTO(10, "Santiago", 1));

        validation.Verify(x => x.ValidateUpdateRequestAsync(10, "Santiago", 1), Times.Once);
        Assert.That(result.Id, Is.EqualTo(10));
        Assert.That(result.Name, Is.EqualTo("Santiago"));
    }

    [Test]
    public async Task DeleteAsync_ShouldDelegateToAccess()
    {
        var validation = new Mock<ICityValidationEngine>();
        var access = new Mock<ICityAccess>();

        ICityManager sut = new CityManager(validation.Object, access.Object);

        await sut.DeleteAsync(7);

        access.Verify(x => x.DeleteAsync(7), Times.Once);
    }
}