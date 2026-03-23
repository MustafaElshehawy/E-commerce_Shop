using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }

        public void update(Product product)
        {
            var ProductInDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (ProductInDb != null)
            {
                ProductInDb.Name = product.Name;
                ProductInDb.Description = product.Description;
                ProductInDb.Img = product.Img;
                ProductInDb.CategoryId = product.CategoryId;
            }
        }
        
    }
}
