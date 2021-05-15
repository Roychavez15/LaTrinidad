using LaTrinidad.Common.Enums;
using LaTrinidad.Web.Data.Entities;
using LaTrinidad.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            //await CheckProvinciaAsync();

            await CheckUserAsync("0123456789", "Admin", "Admin", "admin@hotmail.com", "000000000", "Barrio", UserType.Admin);
            await CheckUserAsync("4040", "Usuario", "Usuario", "user@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
        }
        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.Doctor.ToString());
        }

        //private async Task CheckProvinciaAsync()
        //{
        //    if (!_context.Provincias.Any())
        //    {
        //        AddProvincia("Azuay");
        //        AddProvincia("Bolívar");
        //        AddProvincia("Cañar");
        //        AddProvincia("Carchi");
        //        AddProvincia("Chimborazo");
        //        AddProvincia("Cotopaxi");
        //        AddProvincia("El Oro");
        //        AddProvincia("Esmeraldas");
        //        AddProvincia("Galápagos");
        //        AddProvincia("Guayas");
        //        AddProvincia("Imbabura");
        //        AddProvincia("Loja");
        //        AddProvincia("Los Ríos");
        //        AddProvincia("Manabí");
        //        AddProvincia("Morona Santiago");
        //        AddProvincia("Napo");
        //        AddProvincia("Orellana");
        //        AddProvincia("Pastaza");
        //        AddProvincia("Pichincha");
        //        AddProvincia("Santa Elena");
        //        AddProvincia("Santo Domingo de los Tsáchilas");
        //        AddProvincia("Sucumbíos");
        //        AddProvincia("Tungurahua");
        //        AddProvincia("Zamora Chinchipe");
        //        await _context.SaveChangesAsync();
        //    }
        //}
        //private void AddProvincia(string name)
        //{
        //    _context.Provincias.Add(new Provincia { Nombre = name });
        //}        
    }
}
