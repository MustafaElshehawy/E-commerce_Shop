using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var expiredDeals = _unitOfWork.Product.GetAll(p => p.IsHotDeal && p.EndTime < DateTime.Now).ToList();

            if (expiredDeals.Any())
            {
                expiredDeals.ForEach(p => p.IsHotDeal = false);
                _unitOfWork.Complate();
            }
            
            HomeVM homeVM = new HomeVM()
            {

                Categories = _unitOfWork.Category.GetAll().ToList(),
                HotDealsProducts = _unitOfWork.Product.GetAll(p => p.IsHotDeal == true).ToList(),
                GiftBoxesProducts = _unitOfWork.Product.GetAll(p => p.Category.Name == "Gift Boxes").ToList(),
                PersonalizedProducts = _unitOfWork.Product.GetAll(p => p.Category.Name == "Personalized").ToList(),

            };

            return View(homeVM);
        }

        public IActionResult Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return RedirectToAction("Index");
            }
       
            var results = _unitOfWork.Product.GetAll(
                p => p.Name.ToLower().Contains(term.ToLower()) || p.Description.ToLower().Contains(term.ToLower()),
                Includeword: "Category"
            );

            ViewBag.SearchTerm = term;
            ViewBag.TotalResults = results.Count();

            return View("Search", results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
