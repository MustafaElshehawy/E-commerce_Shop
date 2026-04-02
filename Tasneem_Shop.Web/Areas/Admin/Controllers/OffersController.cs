using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;
using Utilities;

namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class OffersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Offer(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ProductInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (ProductInDb == null)
            {
                return NotFound();
            }
           
            return View(ProductInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Offer(Product product)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Description");
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.UpdateOffer(product);
                _unitOfWork.Complate();

                TempData["message"] = "Offer  Saved succesfully";
                return RedirectToAction("Index","Product");

            }
            
            return View(product);

        }
    }
}
