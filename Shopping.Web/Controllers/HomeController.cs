using Shopping.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Shopping.Services;
using Shopping.Repositories;
using Shopping.Factories;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBasketService _basketService;
        private readonly IHomeViewModelFactory _homeViewModelFactory;

        public HomeController(ILogger<HomeController> logger,
            IBasketService basketService,
            IHomeViewModelFactory homeViewModelFactory)
        {
            _logger = logger;
            _basketService = basketService;
            _homeViewModelFactory = homeViewModelFactory;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            var basket = _basketService.GetCurrentBasket(HttpContext.Request);

            if (basket == null) 
                basket = _basketService.CreateNewBasket();

            if (basket == null)
            {
                _logger.LogError($"Failed to obtain a basket in {nameof(HomeController)}");
            }
            else
            {
                viewModel = _homeViewModelFactory.Create(basket);

                ViewData[Constants.BasketCookieName] = basket.Id;
                ViewData[Constants.BasketQuantity] = viewModel.Basket?.Products?.Count() ?? 0;
            }

            return View(viewModel);
        }
    }
}