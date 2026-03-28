using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels.Customer;

namespace Tasneem_Shop.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                
                Categories=_unitOfWork.Category.GetAll(),
                HotDealsProducts = _unitOfWork.Product.GetAll(),
                GiftBoxesProducts = _unitOfWork.Product.GetAll(p=>p.Category.Name == "Gift Boxes"),
                PersonalizedProducts = _unitOfWork.Product.GetAll(p => p.Category.Name == "Personalized"),

            };

            return View(homeVM);
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
