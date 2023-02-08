using Shopping.Settings;
using Shopping.Tests.Mocks.Factories;
using System.Net.Http.Json;
using Shopping.Models.Entities;

namespace Shopping.Tests.IntegrationTests
{
    [TestFixture]
    public class BasketApiTests
    {
        private HttpClient _client;
        private ApiEndpoints _apiEndpoints;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:7070");
            _apiEndpoints = ServicesFactory.GetApiOptions().Value;
        }

        [Test]
        public void TestGetBasket()
        {
            var endpoint = _apiEndpoints.GetBasket;

            var response = _client.GetAsync(endpoint).Result;

            response.EnsureSuccessStatusCode();

            var basket = response.Content.ReadFromJsonAsync<Basket>().Result;

            Assert.That(basket, Is.Not.Null);
            Assert.That(basket.Id, Is.Not.EqualTo(default(int)));
        }
    }
}
