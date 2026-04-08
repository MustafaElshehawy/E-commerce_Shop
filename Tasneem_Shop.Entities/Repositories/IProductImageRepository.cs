using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IProductImageRepository :IGenericRepository<ProductImage>
    {
        void Update(ProductImage productImage);
    }
}
