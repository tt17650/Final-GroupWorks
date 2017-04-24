using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myTaiwanTest.Models
{
    public class locations
    {
        public int txtID { get; set; }
        public string txtTitle { get; set; }
        public string txtText { get; set; }
        public System.DateTime txtCreateTime { get; set; }
        public System.DateTime txtUpdateTime { get; set; }
        public int userID { get; set; }
        public int location { get; set; }
        public string locationDescription { get; set; }

        public string city { get; set; }

    }
}