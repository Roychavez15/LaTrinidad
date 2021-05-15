using LaTrinidad.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Data.Entities
{
    public class AppointmentEntity
    {
        public int Id { get; set; }
        public UserEntity UserEntity { get; set; }
        public UserEntity Doctor { get; set; }
        public PacientEntity PacientEntity {get; set;}
        public DateTime DateAppoint { get; set; }  
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public AppointmentType AppointmentType { get; set; }

    }
}
