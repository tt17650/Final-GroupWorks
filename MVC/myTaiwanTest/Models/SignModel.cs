using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myTaiwanTest.Models
{
    public class SignModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string passwords { get; set; }
        public string email { get; set; }
        public string faceUrl { get; set; }
        public string coverUrl { get; set; }
    }
}