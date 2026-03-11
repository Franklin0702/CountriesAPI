using App.Access.Countries;
using App.Engine.Countries;
using Moq;
using NUnit.Framework;

namespace App.Tests.Engine;

[TestFixture]
public class CountryValidationEngineTests
{
    [Test]
    public async Task ValidateCreateRequestAsync_ShouldPass_WhenDataIsValid()
    {
        var access = new Mock<ICountryAccess>();
        access.Setup(x => x.IsCodeUniqueAsync("DO")).ReturnsAsync(true);

        var sut = new CountryValidationEngine(access.Object);

        Assert.DoesNotThrowAsync(async () =>
            await sut.ValidateCreateRequestAsync("Dominican Republic", "DO", 18, -70));
    }

    [Test]
    public void ValidateCreateRequestAsync_ShouldThrow_WhenCodeIsInvalid()
    {
        var access = new Mock<ICountryAccess>();
        var sut = new CountryValidationEngine(access.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateCreateRequestAsync("Dominican Republic", "D01", 18, -70));
    }

    [Test]
    public void ValidateCreateRequestAsync_ShouldThrow_WhenCodeIsDuplicated()
    {
        var access = new Mock<ICountryAccess>();
        access.Setup(x => x.IsCodeUniqueAsync("DO")).ReturnsAsync(false);

        var sut = new CountryValidationEngine(access.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateCreateRequestAsync("Dominican Republic", "DO", 18, -70));
    }

    [Test]
    public void ValidateCreateRequestAsync_ShouldThrow_WhenLatitudeIsOutOfRange()
    {
        var access = new Mock<ICountryAccess>();
        access.Setup(x => x.IsCodeUniqueAsync("DO")).ReturnsAsync(true);

        var sut = new CountryValidationEngine(access.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateCreateRequestAsync("Dominican Republic", "DO", 100, -70));
    }

    [Test]
    public async Task ValidateUpdateRequestAsync_ShouldThrow_WhenCountryDoesNotExist()
    {
        var access = new Mock<ICountryAccess>();
        access.Setup(x => x.ExistsAsync(10)).ReturnsAsync(false);

        var sut = new CountryValidationEngine(access.Object);

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await sut.ValidateUpdateRequestAsync(10, "Dominican Republic", "DO"));
    }
}