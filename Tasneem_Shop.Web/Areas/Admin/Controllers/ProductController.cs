using Microsoft.AspNetCore.Mvc;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.categorys =new SelectList(_unitOfWork.Category.GetAll(),"Id","Name");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Complate();
                TempData["message"] = "Data Has created succesfully";

                return RedirectToAction("Index");
            }
            ViewBag.categorys = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name");
            return View(product);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
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
            ViewBag.categorys = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name");

            return View(ProductInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.update(product);
                _unitOfWork.Complate();

                TempData["message"] = "Data Has Updated succesfully";
                return RedirectToAction("Index");

            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(int? id)
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
            _unitOfWork.Product.Remove(ProductInDb);
            _unitOfWork.Complate();
            TempData["message"] = "Data Has Delete succesfully";
            return RedirectToAction("Index");

        }
    }
}
