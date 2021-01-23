using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class MoviesAndHallsDal
    {
        public HallDal Halls { get; set; }
        public MovieDal Movies { get; set; }
    }
}