using BusinessPlanning.Data;
using BusinessPlanning.Entities;
using BusinessPlanning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager;
        private ApplicationDbContext _context;
        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signManager, RoleManager<AppRole> roleManager, ApplicationDbContext context)
        {
            _userManager = usermanager;
            _signInManager = signManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            AppUser user = new AppUser()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsManager = model.IsManager,
                Color = model.Color

            };

            IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                bool roleCheck = model.IsManager ? AddRole("Manager") : AddRole("User");

                if (!roleCheck)
                {
                    return View("Error");


                }
                _userManager.AddToRoleAsync(user, model.IsManager ? "Manager" : "User").Wait();

                return RedirectToAction("Login", "Account");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("CustomError", "Kullanıcı Bulunamadı");
                return View(model);
            }

           
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
            }

            ModelState.AddModelError("CustomError", "Kullanıcı Adı veya Şifresi Hatalı");


            return View(model);
        }


        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync().Wait();


            return RedirectToAction("Login");
        }


        private bool AddRole(string rolName)
        {
            if (!_roleManager.RoleExistsAsync(rolName).Result)
            {
                AppRole role = new AppRole()
                {
                    Name = rolName
                };

                IdentityResult result = _roleManager.CreateAsync(role).Result;

                return result.Succeeded;
            }
            return true;
        }


        public ActionResult UserProfile()
        {
            var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            UserProfileModelView umodel = new UserProfileModelView()
            {
                UserId= user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Color = user.Color,
                CompletedTask = _context.AppTasks.Where(a => a.AppUserId == user.Id && a.IsCompleted == true).Count(),
                DeletedTask= _context.AppTasks.Where(a => a.AppUserId == user.Id && a.IsDeleted == true).Count(),
                AwaitingTask= _context.AppTasks.Where(a => a.AppUserId == user.Id && a.IsCompleted == false&&a.IsDeleted==false).Count(),


            };

            return View(umodel);
        }

        public async Task<JsonResult> UpdateUserProfile( UserProfileModelView model)
        {
            bool usernameChanged = false;
            var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            if (model.UserName!=user.UserName)
            {
                usernameChanged = true;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Color = model.Color;

          


            var result=  await _userManager.UpdateAsync(user);

            if (result.Succeeded&&usernameChanged==false)
            {
                return Json("200");
            }
            else if(result.Succeeded&&usernameChanged)
            {
                return Json("210");
            }

            return Json("Can not Updated");
        }

        public async Task<JsonResult> UpdatePassword(PasswordModelView model)
        {
            var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            if (model.NewPassword!=model.rePassword)
            {

                return Json("220");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Json("200");
            }
            else
            {
                return Json("210");
            }
            
            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            //var resultm = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

          
        }
    }
}
