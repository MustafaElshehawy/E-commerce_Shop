using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.ViewModels;
using Utilities;

namespace Tasneem_Shop.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var claimsIdentity =(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var users = _unitOfWork.User.GetAll(userId);
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UserVM userVM = new UserVM()
            {
                User =new ApplicationUser(),
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text =x.Name,
                    Value=x.Name,
                })

            };
          return View(userVM);
        }
        public async Task<IActionResult> Create(UserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.User.Email, Email = model.User.Email, Name = model.User.Name, Address = model.User.Address, City = model.User.City };
                var userResult = await _userManager.CreateAsync(user, model.Password);

                if (userResult.Succeeded)
                {
                    try
                    { 
                        var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

                        if (!roleResult.Succeeded)
                        {
                            await _userManager.DeleteAsync(user);
                            ModelState.AddModelError("", "Faild To Add Role");
                        }
                        else
                        {
                            TempData["message"] = "User Created Successfully!";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError("", "Error Exception : " + ex.Message);
                    }                   
                }
  
                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.RoleList = _roleManager.Roles.Select(i => new SelectListItem { Text = i.Name, Value = i.Name });
            return View(model);
        }


        public IActionResult LockUnlock(string id)
        {
            var user = _unitOfWork.User.GetFirstOrDefault(id);
            if (user == null)
            {

                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddDays(30);
            }
            else
            { 
                user.LockoutEnd = DateTime.Now;
            
            }

            _unitOfWork.Complate();


            return RedirectToAction("Index", "User");
        }

    }
}
