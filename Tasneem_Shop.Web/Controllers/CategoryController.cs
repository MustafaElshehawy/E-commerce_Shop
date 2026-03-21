using Microsoft.AspNetCore.Mvc;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {   
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        { 
            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complate();
                TempData["message"] = "Data Has created succesfully";

                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstOrDefault(x=>x.Id ==id);
            if (categoryIndb == null)
            {
                return NotFound();

            }
            return View(categoryIndb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
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
            var categoryIndb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryIndb);
            _unitOfWork.Complate();
            TempData["message"] = "Data Has Delete succesfully";
            return RedirectToAction("Index");

        }
       
    }
}
