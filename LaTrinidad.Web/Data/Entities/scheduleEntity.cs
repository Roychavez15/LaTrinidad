using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Data.Entities
{
    public class scheduleEntity
    {
        public int Id { get; set; }
        public UserEntity UserEntity { get; set; }
        public int Day { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Interval { get; set; }
        
    }
}
