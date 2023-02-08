using Shopping.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Shopping.Factories;
using Shopping.Services;

namespace Shopping.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketService _basketService;
        private readonly IBasketViewModelFactory _basketViewModelFactory;

        public BasketController(ILogger<BasketController> logger, 
            IBasketService basketService, 
            IBasketViewModelFactory productViewModelFactory)
        {
            _logger = logger;
            _basketService = basketService;
            _basketViewModelFactory = productViewModelFactory;
        }

        public IActionResult Index()
        {
            var viewModel = new BasketViewModel();

            var basket = _basketService.GetCurrentBasket(HttpContext.Request);

            if (basket == null)
                basket = _basketService.CreateNewBasket();

            if (basket == null)
            {
                _logger.LogError($"Failed to obtain a basket in {nameof(HomeController)}");
            }
            else
            {
                ViewData[Constants.BasketCookieName] = basket.Id;

                viewModel = _basketViewModelFactory.Create(basket);
            }

            ViewData[Constants.BasketQuantity] = viewModel.Products?.Count() ?? 0;

            return View(viewModel);
        }
    }
}
