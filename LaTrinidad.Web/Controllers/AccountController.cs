using LaTrinidad.Common.Enums;
using LaTrinidad.Web.Data.Entities;
using LaTrinidad.Web.Helpers;
using LaTrinidad.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;

        public AccountController(
            IUserHelper userHelper,
            IImageHelper imageHelper,
            ICombosHelper combosHelper)
        {
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserRoleViewModel>> Index()
        {
            var user = await _userHelper.GetListUsers();

            var model = await ToUserRoleViewModel(user.ToList());

            return View(model);
        }

        private async Task<IEnumerable<UserRoleViewModel>> ToUserRoleViewModel(List<UserEntity> users)
        {
            var model = new List<UserRoleViewModel>();
            //throw new NotImplementedException();
            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    Document = user.Document,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    PicturePath = user.PicturePath,
                    //isActive = user.isActive,
                };
                var usermodel = await _userHelper.GetUserAsync(user.UserName);
                var roles = await _userHelper.GetAllListRoles(usermodel);
                userRoleViewModel.Roles = roles.ToList();
                model.Add(userRoleViewModel);
            }

            return model;
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = Guid.Parse(user.Id),
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.PicturePath,
                //Teams = _combosHelper.GetComboTeams(),
                //TeamId = user.Team.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.PicturePath;

                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Users");
                }

                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PicturePath = path;
                //user.Team = await _context.Teams.FindAsync(model.TeamId);

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            //model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                //Teams = _combosHelper.GetComboTeams()
                Roles = _combosHelper.GetComboRoles()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Users");
                }
                UserEntity user;
                if (model.RoleId == "Admin")
                {
                    user = await _userHelper.AddUserAsync(model, path, UserType.Admin);
                }
                else
                {
                    user = await _userHelper.AddUserAsync(model, path, UserType.User);
                }

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Roles = _combosHelper.GetComboRoles();
                    return View(model);
                }
                return RedirectToAction("Index", "Account");
                //LoginViewModel loginViewModel = new LoginViewModel
                //{
                //    Password = model.Password,
                //    RememberMe = false,
                //    Username = model.Username
                //};

                //var result2 = await _userHelper.LoginAsync(loginViewModel);

                //if (result2.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }

            model.Roles = _combosHelper.GetComboRoles();
            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> EditUser(Guid id)
        {
            UserEntity user = await _userHelper.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = Guid.Parse(user.Id),
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.PicturePath,
                Roles = _combosHelper.GetComboRoles(),
                RoleId = user.UserType.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.PicturePath;

                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Users");
                }

                UserEntity user = await _userHelper.GetUserAsync(Guid.Parse(model.Id.ToString()));

                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PicturePath = path;
                if (model.RoleId == "Admin")
                {
                    user.UserType = UserType.Admin;
                }
                user.UserType = UserType.User;
                //user.Team = await _context.Teams.FindAsync(model.TeamId);

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Account");
            }

            model.RoleId = model.RoleId;
            model.Roles = _combosHelper.GetComboRoles();
            return View(model);
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }

    }
}
