using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class OrderHeaderRepository :GenericRepository<OrderHeader> ,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus)
        {
            var orderfromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderfromDb != null)
            {
                orderfromDb.OrderStatus = OrderStatus;
                orderfromDb.PaymentDate = DateTime.Now;
                if (PaymentStatus != null)
                {
                    orderfromDb.PaymentStatus = PaymentStatus;
                    
                }
            
            }
        }
    }
}
