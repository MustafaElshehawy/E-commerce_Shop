using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Security.Claims;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.Servies;
using Tasneem_Shop.Entities.ViewModels;
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
                    Price = (item.Product.OfferPrice > 0 && item.Product.OfferPrice < item.Product.Price) ? (item.Product.OfferPrice ?? 0) : item.Product.Price,
                    ImageUrl = item.Product.Img,
                    Quantity = item.Count
                }).ToList();

                var cartItems = new CartVM
                {
                    CartItems = displayItemsDb,
                    TotalPrice = displayItemsDb.Sum(item => item.Price * item.Quantity)
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
                            Price = (product.OfferPrice > 0 && product.OfferPrice < product.Price)? (product.OfferPrice ?? 0) : product.Price,
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

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

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

            var user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == userId);

            var cartItems = new CartVM
            {
                CartItems = displayItemsDb,
                TotalPrice = displayItemsDb.Sum(item => item.Price * item.Quantity),
                OrderHeader = new OrderHeader()
            };


            cartItems.OrderHeader.Name = user.Name;
            cartItems.OrderHeader.Address = user.Address;
            cartItems.OrderHeader.City = user.City;
            cartItems.OrderHeader.Phone = user.PhoneNumber;
            return View(cartItems);

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(CartVM cartVM)
        {
            //1-fetch user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //2-fetch cart items (## price)
            var cartItemsDb = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId, Includeword: "Product");

            //3-change status and order time
            cartVM.OrderHeader.ApplicationUserId = userId;
            cartVM.OrderHeader.OrderDate = DateTime.Now;
            cartVM.OrderHeader.OrderStatus = SD.Pending;
            cartVM.OrderHeader.PaymentStatus = SD.Pending;

            //4-validate price
            cartVM.OrderHeader.TotalPrice = cartItemsDb.Sum(item => item.Product.Price * item.Count);

            //5- save and  take orderheader id to  order details
            _unitOfWork.OrderHeader.Add(cartVM.OrderHeader);
            _unitOfWork.Complate();

            //6- orderdetail
            foreach (var item in cartItemsDb)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = cartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count

                };
                _unitOfWork.OrderDetail.Add(orderDetail);
            }

            //
            var domain = "https://tasneem-shop.azurewebsites.net/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={cartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            //data
            foreach (var item in cartItemsDb)
            {

                var sessionlineoption = new SessionLineItemOptions
                {

                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);

            }

            var service = new SessionService();
            Session session = service.Create(options);

            
            cartVM.OrderHeader.SessionId = session.Id;
            _unitOfWork.Complate();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            


        }

        public IActionResult orderconfirmation(int id)
        {
            //check paid from Stripe
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeader.PaymentIntentId = session.PaymentIntentId;

                _unitOfWork.OrderHeader.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                _unitOfWork.Complate();
            
            }
            //7 - remove old shopping cart itesm that checkout
            var shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Complate();
            return View(id);

        }
    }
}
