using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class ProductImageRepository:GenericRepository<ProductImage>,IProductImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductImageRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(ProductImage productImage)
        {
            var ImageInDb = _context.ProductImages.FirstOrDefault(x => x.Id == productImage.Id);
            if (ImageInDb != null)
            {
                ImageInDb.ImageUrl = productImage.ImageUrl;
                ImageInDb.ProductId = productImage.ProductId;
            }
        }
    }
}
