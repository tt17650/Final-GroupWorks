using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using myTaiwanTest.Models;

namespace myTaiwanTest.Models
{
    public class AddfriendPlusText
    {
        //for friendPage and _FriendLayout || for _DynamicLayout and DynamicIndex
        private myTaiwanEntities db = new myTaiwanEntities();

        public IEnumerable<sp_BrowseText_Result> browseText{ set; get; }
        public IEnumerable<sp_BrowseTextbyCounty_Result> browseTextbyCounty { set; get; }
        
        public Sign sign { set; get; }
        public Sign FriendSign { set; get; }
        public int friendID { set; get; }
        //傳入使用者與好友，判斷是否為追蹤關係
        public bool isFriend(int userID , int friendID)
        {
            var checkIsFriend = db.Friends.FirstOrDefault(o => o.userID == userID && o.friendID == friendID);
            if (checkIsFriend != null)
                return true;
            else
                return false;
        }
        //傳入使用者，判斷瀏覽對象是否為追蹤關係
        public bool isFriend(int userID)
        {
            var checkIsFriend = db.Friends.FirstOrDefault(o => o.userID == userID && o.friendID == friendID);
            if (checkIsFriend != null)
                return true;
            else
                return false;
        }

    }
}