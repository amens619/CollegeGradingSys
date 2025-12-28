

using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
//using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class AdminstrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
       
        private readonly ApplicationDbContext db;

        public AdminstrationController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext _db
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
           
            db = _db;
        }

        
        [HttpGet]
        [Authorize(Policy = "ManageUserClaimsPolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {userId} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن التعديل على هذاالمستخدم";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var model = new UserClaimsViewModel
            {
                UserRoles = roles,
                UserId = userId,
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                };

                if(existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);

        }
        [HttpPost]
        [Authorize(Policy = "ManageUserClaimsPolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {model.UserId} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن التعديل على هذاالمستخدم";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user,claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لا يمكن حذف صلاحيات المستخدم");
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user, model.Claims.Where(x => x.IsSelected).Select(y => new Claim(y.ClaimType,y.ClaimValue )));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لا يمكن إضافة صلاحيات للمستخدم");
                return View(model);
            }




            return RedirectToAction("EditUser", new { Id = model.UserId });



        }
        [HttpGet]
        [Authorize(Policy = "ManageUserRolesPolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {userId} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن التعديل على هذاالمستخدم";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();
            foreach (var role in roleManager.Roles)
            {
                if (role.Name !="Admin")
                {
                    var userRolesViewModel = new UserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRolesViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRolesViewModel.IsSelected = false;
                    }

                    model.Add(userRolesViewModel);
                }
               
            }

            return View(model);

        }
        [HttpPost]
        [Authorize(Policy = "ManageUserRolesPolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model,string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {userId} غير موجود";
                return View("NotFound");
            }

            var roles =await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لا يمكن حذف ادوار او مسؤليات المستخدم");
                return View(model);
            }          

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لا يمكن إضافة ادوار او مسؤليات للمستخدم");
                return View(model);
            }




            return RedirectToAction("EditUser", new { Id = userId });
        }
    
        [HttpPost]
        [Authorize(Policy = "DeleteUserPolicy")]
        public async Task<IActionResult> DeleteUser(string id)
        { 
        var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {id} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن حذف هذاالمستخدم";
                return View("NotFound");
            }
            else
            { 
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {

                        case "DuplicateUserName":
                            ModelState.AddModelError("", user.UserName + "   موجود مسبقا.");
                            break;

                        default:
                            ModelState.AddModelError("", error.Description);
                            //case "":
                            break;
                    }

                }
                return View("ListUsers");

            }
        }
            [HttpGet]
        public IActionResult ListUsers()
        {
           var users = GetListUsers();

            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = "EditUserPolicy")]
        public async Task<IActionResult> EditUser(string id)
        {

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {id} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن التعديل في هذاالمستخدم";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            
          
             var model = new EditeUserViewModel
            {
                Id = user.Id,   
                Email = user.Email,            
                Roles= (List<string>)userRoles       
                 
            };

            //=========================

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }
            //===========================
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditUserPolicy")]
        public async Task<IActionResult> EditUser(EditeUserViewModel model)
        {

            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"رقم المستخدم  {model.Id} غير موجود";
                return View("NotFound");
            }
            else if (user.UserName == "Admin")
            {
                ViewBag.ErrorMessage = $"لا يمكن التعديل في هذاالمستخدم";
                return View("NotFound");
            }
            else 
            {
               
               
                  
                  
              
                if (ModelState.IsValid)
                {
                    user.Email = model.Email;
                   
                    user.UserName = model.Email;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        switch (error.Code)
                        {

                            case "DuplicateUserName":
                                ModelState.AddModelError("", user.UserName + "   موجود مسبقا.");
                                break;

                            default:
                                ModelState.AddModelError("", error.Description);
                                //case "":
                                break;
                        }

                    }
                }
            }
            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var NewModel = new EditeUserViewModel();
            //model.Claims = userClaims.Select(c => c.Value).ToList();
            //=========================

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                NewModel.Claims.Add(userClaim);
            }
            //===========================
         
            NewModel.Id = model.Id;          
            NewModel.Email = model.Email;
            NewModel.Roles = (List<string>)userRoles;    
            
           
            return View(NewModel);
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]       
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole() { Name = model.RoleName };
                IdentityResult result = await roleManager.CreateAsync(identityRole);


                if (result.Succeeded)
                {

                    return RedirectToAction("ListRoles", "Adminstration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateRoleName":
                            ModelState.AddModelError("", identityRole.Name + "   موجود مسبقا.");
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


        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]       
        public async Task<IActionResult> EditRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"رقم المسؤولية  {id} غير موجود";
                return View("NotFound");
            }
            var model = new EditeRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditeRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"رقم المسؤولية  {model.Id} غير موجود";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {

                        case "DuplicateRoleName":
                            ModelState.AddModelError("", role.Name + "   موجود مسبقا.");
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


        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"رقم المسؤولية  {roleId} غير موجود";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName                  
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {

           
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"رقم المسؤولية  {roleId} غير موجود";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
              var user = await  userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }else if (!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name)) {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if(i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        RedirectToAction("EditRole", new { Id = roleId });
                    }
                   
                }

            }


            return RedirectToAction("EditRole",new { Id = roleId});
        }


       

      

        public IList<IdentityUser> GetListUsers()
        {
            return db.Users.ToList();
        }
    }
}
