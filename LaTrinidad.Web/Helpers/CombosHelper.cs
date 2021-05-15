using LaTrinidad.Common.Enums;
using LaTrinidad.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CombosHelper(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        //public IEnumerable<SelectListItem> GetComboEstados()
        //{
        //    var myList = new List<SelectListItem>();

        //    myList = new List<SelectListItem>
        //            {
        //                new SelectListItem {Text = "EFECTIVO", Value =  "EFECTIVO"},
        //                new SelectListItem {Text = "NO EFECTIVO", Value =  "NO EFECTIVO"},
        //                new SelectListItem {Text = "NO CONTACTADO", Value =  "NO CONTACTADO"},
        //            };

        //    return myList;
        //}
        
        public async Task<IEnumerable<SelectListItem>> GetComboUsersByRole(string Role)
        {
            List<SelectListItem> myList = new List<SelectListItem>();

            var Listaclientes = await _userHelper.GetListUsersInRole(Role);

            int id = 1;
            foreach (var Data in Listaclientes)
            {

                var data =
                 new SelectListItem
                 {
                     Value = Data.Id,
                     Text = Data.FullName,
                 };
                myList.Add(data);
                id = id + 1;
            };


            myList.Insert(0, new SelectListItem
            {
                Text = "[Seleccionar...]",
                Value = ""
            });

            return myList.OrderBy(n => n.Text);
            //throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            List<SelectListItem> myList = new List<SelectListItem>();


            foreach (var rol in Enum.GetNames(typeof(UserType)))
            {
                var data =
                     new SelectListItem
                     {
                         Value = rol,
                         Text = rol,
                     };
                myList.Add(data);
            }

            myList.Insert(0, new SelectListItem
            {
                Text = "[Seleccionar...]",
                Value = ""
            });

            return myList.OrderBy(n => n.Text);
            //throw new NotImplementedException();
        }
                
    }
}
