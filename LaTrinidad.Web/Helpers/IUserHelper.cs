using LaTrinidad.Common.Enums;
using LaTrinidad.Web.Data.Entities;
using LaTrinidad.Web.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Helpers
{
    public interface IUserHelper
    {
        Task<IEnumerable<RoleViewModel>> GetAllListRoles(UserEntity user);
        IEnumerable<RoleViewModel> GetAllListRoles();
        Task<IEnumerable<UserEntity>> GetListUsers();
        Task<UserEntity> GetUserAsync(Guid userId);
        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(UserEntity user);


        Task<UserEntity> AddUserAsync(AddUserViewModel model, string path, UserType userType);
        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<UserEntity> GetUserByIdAsync(string Id);
        Task<IEnumerable<UserEntity>> GetListUsersInRole(string Role);
        Task<UserEntity> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(UserEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(UserEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);
    }
}
