using LaTrinidad.Common.Enums;
using LaTrinidad.Web.Data;
using LaTrinidad.Web.Data.Entities;
using LaTrinidad.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserEntity> _signInManager;
        public UserHelper(
            DataContext context,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserEntity> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            return await _context.Users
                //.Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }
        public async Task<UserEntity> GetUserAsync(string email)
        {
            return await _context.Users
                //.Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEntity user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<UserEntity> AddUserAsync(AddUserViewModel model, string path, UserType userType)
        {
            UserEntity userEntity = new UserEntity
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PicturePath = path,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                UserType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(userEntity, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            UserEntity newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, userEntity.UserType.ToString());
            return newUser;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(UserEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        //public async Task<UserEntity> GetUserAsync(string email)
        //{
        //    return await _userManager.FindByEmailAsync(email);
        //}

        public async Task<UserEntity> GetUserByIdAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }
        public async Task<bool> IsUserInRoleAsync(UserEntity user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
        public async Task<IEnumerable<UserEntity>> GetListUsersInRole(string Role)
        {
            return await _userManager.GetUsersInRoleAsync(Role);

            //throw new System.NotImplementedException();
        }
        public async Task<IEnumerable<UserEntity>> GetListUsers()
        {
            return await _context.Users.ToListAsync();

            //throw new System.NotImplementedException();
        }
        public IEnumerable<RoleViewModel> GetAllListRoles()
        {
            var roles = _roleManager.Roles.OrderBy(r => r.Name);
            var model = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                };
                roleViewModel.IsSelected = false;
                model.Add(roleViewModel);
            }
            return model;
            //throw new System.NotImplementedException();
        }
        public async Task<IEnumerable<RoleViewModel>> GetAllListRoles(UserEntity user)
        {
            var roles = _roleManager.Roles.OrderBy(r => r.Name);
            var model = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roleViewModel.IsSelected = true;
                }
                else
                {
                    roleViewModel.IsSelected = false;
                }
                model.Add(roleViewModel);
            }
            return model;
            //throw new System.NotImplementedException();
        }
    }
}
