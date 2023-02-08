using Shopping.Factories;
using Shopping.Models.Entities;
using Shopping.Tests.Mocks.Factories;

namespace Shopping.Tests.UnitTests
{
    [TestFixture]
    public class ViewModelFactoryTests
    {
        private IBasketViewModelFactory _basketViewModelFactory;
        private IProductViewModelFactory _productViewModelFactory;

        [SetUp]
        public void Setup()
        {
            _basketViewModelFactory = ServicesFactory.GetBasketViewModelRepository();
            _productViewModelFactory = ServicesFactory.GetProductViewModelRepository();
        }

        [Test]
        public void CreateProductViewModel()
        {
            var product = new Product();

            var viewModel = _productViewModelFactory.Create(product);

            Assert.That(viewModel.Info, Is.EqualTo(product));
        }

        [Test]
        public void CreateBasketViewModel()
        {
            Basket? basket = null;

            var viewModel = _basketViewModelFactory.Create(basket);

            Assert.That(viewModel.Id, Is.EqualTo(default(int)));
        }
    }
}