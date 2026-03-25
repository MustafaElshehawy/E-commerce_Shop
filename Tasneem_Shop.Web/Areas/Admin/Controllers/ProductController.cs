using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Configuration;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;
using static System.Net.Mime.MediaTypeNames;


namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem { 
                    
                    Text = x.Name,
                    Value =x.Id.ToString(),
                
                })
                
            };
            return View(productVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(ProductVM productVM ,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;//وصلت لفولدر wwwroot
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images\Products");
                    var ext =Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complate();
                TempData["message"] = "Data Has created succesfully";

                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            return View(productVM);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ProductVM productVM = new ProductVM()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString(),

                })

            };
            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM,IFormFile file)
        {
            
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images/Products");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(upload,fileName+ext),FileMode.Create))
                    { 
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + fileName + ext;
                }           
                _unitOfWork.Product.update(productVM.Product);
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
