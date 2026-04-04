using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IShoppingCartRepository :IGenericRepository<ShoppingCart>
    {
        public void IncrementCount(ShoppingCart shoppingCart, int count);

    }
}
