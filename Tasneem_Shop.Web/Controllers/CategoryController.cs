using Microsoft.AspNetCore.Mvc;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories =_context.Categories.ToList();
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
                _context.Categories.Add(category);
                _context.SaveChanges();
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
            var categoryIndb = _context.Categories.Find(id);
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
                _context.Categories.Update(category);
                _context.SaveChanges();
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
            var categoryIndb = _context.Categories.Find(id);
            if (categoryIndb == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categoryIndb);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }
       
    }
}
