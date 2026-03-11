using App.Access.Cities;
using App.Access.Countries;
using App.Engine.Cities;
using Moq;
using NUnit.Framework;

namespace App.Tests.Engine;

[TestFixture]
public class CityValidationEngineTests
{
    [Test]
    public async Task ValidateCreateRequestAsync_ShouldPass_WhenDataIsValid()
    {
        var cityAccess = new Mock<ICityAccess>();
        var countryAccess = new Mock<ICountryAccess>();
        countryAccess.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);

        var sut = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

        Assert.DoesNotThrowAsync(async () =>
            await sut.ValidateCreateRequestAsync("Santo Domingo", 1));
    }

    [Test]
    public void ValidateCreateRequestAsync_ShouldThrow_WhenNameIsEmpty()
    {
        var cityAccess = new Mock<ICityAccess>();
        var countryAccess = new Mock<ICountryAccess>();
        countryAccess.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);

        var sut = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateCreateRequestAsync("", 1));
    }

    [Test]
    public void ValidateCreateRequestAsync_ShouldThrow_WhenCountryDoesNotExist()
    {
        var cityAccess = new Mock<ICityAccess>();
        var countryAccess = new Mock<ICountryAccess>();
        countryAccess.Setup(x => x.ExistsAsync(999)).ReturnsAsync(false);

        var sut = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateCreateRequestAsync("Santo Domingo", 999));
    }

    [Test]
    public void ValidateUpdateRequestAsync_ShouldThrow_WhenCityDoesNotExist()
    {
        var cityAccess = new Mock<ICityAccess>();
        cityAccess.Setup(x => x.ExistsAsync(10)).ReturnsAsync(false);

        var countryAccess = new Mock<ICountryAccess>();
        var sut = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateUpdateRequestAsync(10, "Santiago", 1));
    }
}