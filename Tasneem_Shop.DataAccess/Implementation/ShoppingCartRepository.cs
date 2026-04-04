using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart> ,IShoppingCartRepository
    {
        private ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
        }
    }
}
