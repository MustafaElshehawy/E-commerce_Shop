using System;
using System.Collections.Generic;
using System.Text;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }

        IShoppingCartRepository ShoppingCart { get; }

        int Complate();
    }
}
