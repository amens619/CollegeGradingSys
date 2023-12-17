using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;         
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CreateUserPolicy")]
        public IActionResult Register()
        {
            var RegisterVM = new RegisterViewModel();
            

            return View(RegisterVM);
        }



        [HttpPost]
        [Authorize(Policy = "CreateUserPolicy")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
          
            if (ModelState.IsValid)
            {


                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email                   
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && (User.IsInRole("Admin")))
                    {
                        return RedirectToAction("ListUsers", "Adminstration");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordTooShort":
                            ModelState.AddModelError("", "يجب ألا تقل كلمات المرور عن 10 أحرف.");
                            break;
                        case "PasswordRequiresNonAlphanumeric":
                            ModelState.AddModelError("", "يجب أن تحتوي كلمات المرور على رمز واحد على الأقل .");
                            break;
                        case "DuplicateUserName":
                            ModelState.AddModelError("", user.Email + "   موجود مسبقا.");
                            break;

                        default:
                            ModelState.AddModelError("", error.Description);
                            //case "":
                            break;
                    }

                }
            }
          
            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null) { return Json(true); }
            else { return Json(Email + "   موجود مسبقا."); }

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "Home");
                    }

                }

                ModelState.AddModelError("", "خطأ في البريد الالكتروني او كلمة المرور");


            }
            return View(model);

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
