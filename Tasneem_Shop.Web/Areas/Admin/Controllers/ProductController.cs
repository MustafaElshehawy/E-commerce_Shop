using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Configuration;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;
using Utilities;
using static System.Net.Mime.MediaTypeNames;


namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
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
        public IActionResult Create(ProductVM productVM ,IFormFile file , List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;//وصلت لفولدر wwwroot
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images/Products");
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images/Products/" + filename + ext;

                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complate();


                if (files != null && files.Count > 0)
                {
                    foreach (var galleryFile in files)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var upload = Path.Combine(RootPath, @"Images/Products/Gallery");
                        if (!Directory.Exists(upload))
                        {
                            Directory.CreateDirectory(upload);
                        }
                        var ext = Path.GetExtension(galleryFile.FileName);

                        using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                        {
                            galleryFile.CopyTo(filestream);
                        }
                        ProductImage productImage = new ProductImage
                        {
                            ImageUrl = @"Images/Products/Gallery/" + filename + ext,
                            ProductId = productVM.Product.Id 
                        };
                        _unitOfWork.ProductImage.Add(productImage);
                    }
                }

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

        //rem add edit images gallery 
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
        public IActionResult Edit(ProductVM productVM,IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images/Products");
                    var ext = Path.GetExtension(file.FileName);
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }
                    if (productVM.Product.Img != null)
                    {
                        var oldImage = Path.Combine(RootPath, productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload,fileName+ext),FileMode.Create))
                    { 
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images/Products/" + fileName + ext;
                }           
                _unitOfWork.Product.update(productVM.Product);
                _unitOfWork.Complate();

                TempData["message"] = "Data Has Updated succesfully";
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
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ProductInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id ,Includeword: "ProductImages");
            if (ProductInDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(ProductInDb);

            //for cover

            var oldImage = Path.Combine(_webHostEnvironment.WebRootPath, ProductInDb.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            //for Gallary
            if (ProductInDb.ProductImages != null)
            {
                foreach (var image in ProductInDb.ProductImages)
                {
                    var galleryPath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(galleryPath))
                    {
                        System.IO.File.Delete(galleryPath);
                    }
                }
            }
            _unitOfWork.Complate();
            TempData["message"] = "Data Has Delete succesfully";
            return RedirectToAction("Index");

        }
    }
}
