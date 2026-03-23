using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        void update(Product product);
    }
}
