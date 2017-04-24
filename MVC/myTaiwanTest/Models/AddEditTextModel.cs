using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using myTaiwanTest.Models;

namespace myTaiwanTest.Models {
    public class AddEditTextModel {
        public Sign sign { get; set; }
        public Text text { get; set; }
        public sp_BrowseSingleText_Result SingleText { get; set; }
    }
}