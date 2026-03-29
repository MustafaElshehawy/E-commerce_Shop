using System;
using System.Collections.Generic;
using System.Text;

namespace Tasneem_Shop.Entities.ViewModels.Customer
{
    public class CartItemDetailsVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
