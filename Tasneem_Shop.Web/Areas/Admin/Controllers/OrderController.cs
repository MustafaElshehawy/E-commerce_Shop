using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Climate;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;
using Utilities;

namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;

            switch (status)
            {
                case "Pending":
                    objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == "Pending", Includeword: "ApplicationUser");
                    break;
                case "Shipped":
                    objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == "Shipped", Includeword: "ApplicationUser");
                    break;
                case "Approve":
                    objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == "Approve", Includeword: "ApplicationUser");
                    break;
                case "Cancelled":
                    objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == "Cancelled", Includeword: "ApplicationUser");
                    break;
                default:
                    objOrderHeaders = _unitOfWork.OrderHeader.GetAll( Includeword: "ApplicationUser");
                    break;
            }

            return View(objOrderHeaders);
        }

        public IActionResult OrderDetails(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id, Includeword: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(o=>o.OrderId ==id, Includeword: "Product")
            };

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {

            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderVM.OrderHeader.Id);


            orderHeaderFromDb.Name = orderVM.OrderHeader.Name;
            orderHeaderFromDb.Phone = orderVM.OrderHeader.Phone;
            orderHeaderFromDb.Address = orderVM.OrderHeader.Address;
            orderHeaderFromDb.City = orderVM.OrderHeader.City;

            if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = orderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Complate();


            TempData["message"] = "Order Details Update success";
            return RedirectToAction("OrderDetails","Order", new { id = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult StartProcessing(OrderVM orderVM)
        {    
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderVM.OrderHeader.Id,SD.Proccessing,null);
            _unitOfWork.Complate();


            TempData["message"] = "Order Proccessing  success";
            return RedirectToAction("OrderDetails", "Order", new { id = orderVM.OrderHeader.Id });

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult StartShiping(OrderVM orderVM)
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderVM.OrderHeader.Id);

             orderHeaderFromDb.Carrier = orderVM.OrderHeader.Carrier;
             orderHeaderFromDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            orderHeaderFromDb.OrderStatus = SD.Shipped;
            orderHeaderFromDb.ShippingDate = DateTime.Now;

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Complate();


            TempData["message"] = "Order Shipped  success";
            return RedirectToAction("OrderDetails", "Order", new { id = orderVM.OrderHeader.Id });


        }


        //cancel and refund
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderVM.OrderHeader.Id);

            if (orderHeaderFromDb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId

                };

                var service = new RefundService();
                Refund refund = service.Create(option);


                _unitOfWork.OrderHeader.UpdateOrderStatus(orderHeaderFromDb.Id, SD.Cancelled, SD.Refund);

            }
            else 
            {
            
                _unitOfWork.OrderHeader.UpdateOrderStatus(orderHeaderFromDb.Id, SD.Cancelled, SD.Cancelled);

            }

            _unitOfWork.Complate();


            TempData["message"] = "Order ha Canceled  success";
            return RedirectToAction("OrderDetails", "Order", new { id = orderVM.OrderHeader.Id });


        }
    }
}
