using NUnit.Framework;
using Moq;
using App.Engine.Cities;
using App.Access.Cities;
using App.Access.Countries;
using System.Threading.Tasks;
using System;

namespace App.Tests
{
    public class CityValidationEngineTests
    {
        [Test]
        public async Task ValidateCreate_Succeeds_WhenCountryExistsAndNameValid()
        {
            var cityAccess = new Mock<ICityAccess>();
            var countryAccess = new Mock<ICountryAccess>();
            countryAccess.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var engine = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

            Assert.DoesNotThrowAsync(async () => await engine.ValidateCreateRequestAsync("Name", 1));
        }

        [Test]
        public void ValidateCreate_Throws_WhenNameInvalid()
        {
            var cityAccess = new Mock<ICityAccess>();
            var countryAccess = new Mock<ICountryAccess>();
            countryAccess.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var engine = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

            Assert.ThrowsAsync<ArgumentException>(async () => await engine.ValidateCreateRequestAsync("   ", 1));
        }

        [Test]
        public void ValidateCreate_Throws_WhenCountryDoesNotExist()
        {
            var cityAccess = new Mock<ICityAccess>();
            var countryAccess = new Mock<ICountryAccess>();
            countryAccess.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var engine = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

            Assert.ThrowsAsync<ArgumentException>(async () => await engine.ValidateCreateRequestAsync("Name", 99));
        }

        [Test]
        public void ValidateUpdate_Throws_WhenCityDoesNotExist()
        {
            var cityAccess = new Mock<ICityAccess>();
            cityAccess.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var countryAccess = new Mock<ICountryAccess>();

            var engine = new CityValidationEngine(cityAccess.Object, countryAccess.Object);

            Assert.ThrowsAsync<ArgumentException>(async () => await engine.ValidateUpdateRequestAsync(5, "Name", 1));
        }
    }
}
