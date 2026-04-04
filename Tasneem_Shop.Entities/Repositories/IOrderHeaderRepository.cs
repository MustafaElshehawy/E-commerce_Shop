using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        //to update only status without update all order prop
        void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus);
    }
}
