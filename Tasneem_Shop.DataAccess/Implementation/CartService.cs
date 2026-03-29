using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Servies;
using Tasneem_Shop.Entities.ViewModels.Customer;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class CartService : ICartService
    {
        public List<CartItemVM> AddToCart(List<CartItemVM> currentCart, int productId, int quantity)
        {
            var productInCookies = currentCart.FirstOrDefault(p => p.ProductId == productId);

            if (productInCookies != null)
            {
                productInCookies.Quantity += quantity;

            }
            else
            {
                currentCart.Add(new CartItemVM { ProductId = productId, Quantity = quantity });
            }

            return currentCart;
        }
    }
}
