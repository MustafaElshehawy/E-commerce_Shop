using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels.Customer;
using Utilities;


namespace Tasneem_Shop.Web.Areas.Customer.ViewComponents
{
    public class ShoppingCartCounterViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartCounterViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            int count = 0;

            if (claim != null)
            {
                count = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == claim.Value).Sum(x => x.Count);

            }
            else
            {
                var cartCookie = Request.Cookies[SD.CookieCartName];

                if (!string.IsNullOrEmpty(cartCookie))
                {

                    var cartList = JsonConvert.DeserializeObject<List<CartItemVM>>(cartCookie);

                    count = cartList?.Sum(x => x.Quantity) ?? 0;
                }


            }

            return View(count);
        }

    }
}
