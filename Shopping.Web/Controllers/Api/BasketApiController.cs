using Shopping.Services;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers.Api
{
    [ApiController]
    [Route("api/basket/[action]")]
    public class BasketApiController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketApiController> _logger;

        public BasketApiController(IBasketService basketService, ILogger<BasketApiController> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }

        [HttpGet(Name = "GetBasket")]
        public JsonResult GetBasket()
        {
            var basket = _basketService.GetCurrentBasket(Request);

            return new JsonResult(basket);
        }

        [HttpPost(Name="add")]
        public IActionResult Add(int basketId, int productId)
        {
            try
            {
                var addedProduct = _basketService.AddProductToBasket(basketId, productId);

                if (addedProduct != null)
                {
                    _logger.LogInformation($"Successfully added product {productId} to basket {basketId}");

                    return new JsonResult(addedProduct);
                }
                else
                {
                    _logger.LogWarning($"Failed to add product {productId} to basket {basketId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred adding product {productId} to basket {basketId}");
            }

            return StatusCode(500);
        }

        [HttpPost(Name = "remove")]
        public IActionResult Remove(int basketId, int productId, int quantity = 1)
        {
            try
            {
                var success = _basketService.RemoveProductFromBasket(basketId, productId, quantity);

                if (success)
                {
                    _logger.LogInformation($"Successfully removed product {productId} from basket {basketId}");

                    return Ok();
                }
                else
                {
                    _logger.LogWarning($"Failed to remove product {productId} from basket {basketId}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred removing product {productId} from basket {basketId}");
            }

            return StatusCode(500);
        }

        [HttpPost(Name = "applyVoucher")]
        public JsonResult ApplyVoucher(int basketId, int voucherId)
        {
            try
            {
                var addedVoucher = _basketService.AddVoucherToBasket(basketId, voucherId);

                if (addedVoucher != null)
                {
                    _logger.LogInformation($"Successfully added voucher {voucherId} to basket {basketId}");

                    return new JsonResult(addedVoucher);
                }
                else
                {
                    _logger.LogWarning($"Failed to add voucher {voucherId} to basket {basketId}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred applying voucher {voucherId} to basket {basketId}");
            }

            return new JsonResult(string.Empty)
            {
                StatusCode = 500
            };
        }

        [HttpPost(Name = "removeVoucher")]
        public IActionResult RemoveVoucher(int basketId)
        {
            try
            {
                var success = _basketService.RemoveVoucherFromBasket(basketId);

                if (success)
                {
                    _logger.LogInformation($"Successfully removed voucher from basket {basketId}");

                    return Ok();
                }
                else
                {
                    _logger.LogWarning($"Failed to remove voucher from basket {basketId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred removing voucher from basket {basketId}");
            }

            return StatusCode(500);
        }
    }
}
