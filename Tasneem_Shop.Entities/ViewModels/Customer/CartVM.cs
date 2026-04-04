using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.ViewModels.Customer
{
    public  class CartVM
    {

        public List<CartItemDetailsVM> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
