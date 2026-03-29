using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tasneem_Shop.DataAccess.Implementation;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.Servies;
using Tasneem_Shop.Entities.ViewModels.Customer;

namespace Tasneem_Shop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ICartService _cartService;

        public CartController(ICartService cartService, IUnitOfWork unitOfWork)
        {
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }
        // i While imporve it after Auth
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            const string cookieName = "Cart";
            List<CartItemVM> cart;

            var existingCookie = Request.Cookies[cookieName];
            if (string.IsNullOrEmpty(existingCookie))
            {
                cart = new List<CartItemVM>();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(existingCookie);
            }

            cart = _cartService.AddToCart(cart, productId, quantity);

            var cartJson = JsonConvert.SerializeObject(cart);

            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Path = "/"
            };

            Response.Cookies.Append(cookieName, cartJson, options);

            return Json(
                new
                {
                    success = true,
                    cartCount = cart.Sum(prod => prod.Quantity)
                });

        }

        // i While imporve it after Auth
        public IActionResult GetCartItems()
        {
            var cookieValue = Request.Cookies["Cart"];
            var cartItems = string.IsNullOrEmpty(cookieValue) ? new List<CartItemVM>() : JsonConvert.DeserializeObject<List<CartItemVM>>(cookieValue);

            var displayItems = new List<CartItemDetailsVM>();
            foreach (var item in cartItems)
            {
                var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    displayItems.Add(new CartItemDetailsVM
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        ImageUrl = product.Img,
                        Quantity = item.Quantity
                    });
                }
            }


            return View(displayItems);

        }


        // i While imporve it  after Auth
        public IActionResult RemoveItemFromCart(int id)
        {
            var cookieValue = Request.Cookies["Cart"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var cartItems = JsonConvert.DeserializeObject<List<CartItemVM>>(cookieValue);

                var itemToRemove = cartItems.FirstOrDefault(u => u.ProductId == id);

                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);

                    var jsonString = JsonConvert.SerializeObject(cartItems);
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        Path = "/"
                    };
                    Response.Cookies.Append("Cart", jsonString, options);
                }
            }

            return RedirectToAction("GetCartItems");



        }
    }  
}
