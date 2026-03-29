using Microsoft.AspNetCore.Mvc;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels.Customer;

namespace Tasneem_Shop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }

        public IActionResult Shop()
        {
            var product = _unitOfWork.Product.GetAll(Includeword: "Category");
            ViewBag.CurrentCategory = "Shop";
            return View(product);
        }
        public IActionResult Details(int id)
        {
            var productDetails = _unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            return View(productDetails);
        }

        [Route("Customer/Product/GetByCategory/{category}")]
        public IActionResult GetByCategory(string category)
        {

            var getProductByName = _unitOfWork.Product.GetAll(p => p.Category.Name == category ,Includeword: "Category");
            ViewBag.CurrentCategory = category;

            return View(getProductByName);
        }
        public IActionResult GetAllHotDeals()
        {

            var getAllHotDeals = _unitOfWork.Product.GetAll(p => p.IsHotDeal, Includeword: "Category");
            ViewBag.CurrentCategory = "HotDeals";

            return View(getAllHotDeals);
        }
    }
}
