using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.ViewModels.Customer
{
    public class HomeVM
    {
        public IEnumerable<Category>? Categories { get; set; }

        public IEnumerable<Product>? HotDealsProducts { get; set; }
        public IEnumerable<Product>? GiftBoxesProducts { get; set; }
        public IEnumerable<Product>? PersonalizedProducts { get; set; }

    }
}
