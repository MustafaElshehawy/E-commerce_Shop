using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Tasneem_Shop.DataAccess.Implementation;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.Servies;
using Tasneem_Shop.Entities.ViewModels.Customer;
using Utilities;

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
        

        public IActionResult AddToCart(int productId, int quantity = 1)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                //Auth User
                var userId = claim.Value;

                var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.UserId == userId && u.ProductId == productId);
                if (cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(new ShoppingCart
                    {
                        UserId = userId,
                        ProductId = productId,
                        Count = quantity
                    });
                }
                else
                {
                    _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, quantity);

                }
                _unitOfWork.Complate();
                var totalCount = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId).Sum(u => u.Count);
               
                return Json(new { success = true, cartCount = totalCount });
            }
            else
            {
                //gest Mode


                const string cookieName = SD.CookieCartName;
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

        }

       
        public IActionResult GetCartItems()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                //Auth User
                var userId = claim.Value;
                var cartItemsDb = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId, Includeword: "Product");
                var displayItemsDb = cartItemsDb.Select(item => new CartItemDetailsVM
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    ImageUrl = item.Product.Img,
                    Quantity = item.Count
                }).ToList();

                var cartItems =new CartVM
                {
                    CartItems=displayItemsDb,
                    TotalPrice= displayItemsDb.Sum(item => item.Price * item.Quantity)
                };
                return View(cartItems);

            }
            else 
            {
                //gest Mode
                var cookieValue = Request.Cookies[SD.CookieCartName];
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
                var cartItemsgest = new CartVM
                {
                    CartItems = displayItems,
                    TotalPrice = displayItems.Sum(item => item.Price * item.Quantity)
                };
                return View(cartItemsgest);
            }      
        }


        
        public IActionResult RemoveItemFromCart(int id)
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                //Auth User
                var userId = claim.Value;
                var cartItem = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.UserId == userId && u.ProductId == id);
                if (cartItem != null)
                {
                    _unitOfWork.ShoppingCart.Remove(cartItem);
                    _unitOfWork.Complate();
                 
                }
                return RedirectToAction("GetCartItems");

            }
            else
            {
                //gest mode
                var cookieValue = Request.Cookies[SD.CookieCartName];

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
                        Response.Cookies.Append(SD.CookieCartName, jsonString, options);
                    }
                }

                return RedirectToAction("GetCartItems");
            }


        }
    }  
}
