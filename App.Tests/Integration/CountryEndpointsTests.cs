using App.Manager.Countries.DTO;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace App.Tests.Integration;

[TestFixture]
public class CountryEndpointsTests
{
    private ApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new ApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task PostCountry_ThenGetCountry_ShouldSucceed()
    {
        var createRequest = new CreateCountryRequestDTO(
            "Dominican Republic",
            "DO",
            18,
            -70);

        var postResponse = await _client.PostAsJsonAsync("/Country", createRequest);

        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var created = await postResponse.Content.ReadFromJsonAsync<CountryDTO>();
        Assert.That(created, Is.Not.Null);

        var getResponse = await _client.GetAsync($"/Country/{created!.Id}");

        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}