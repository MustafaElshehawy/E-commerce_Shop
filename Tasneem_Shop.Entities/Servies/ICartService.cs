using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.ViewModels.Customer;

namespace Tasneem_Shop.Entities.Servies
{
    public interface ICartService
    {
        List<CartItemVM> AddToCart(List<CartItemVM> currentCart, int productId, int quantity);
    }
}
