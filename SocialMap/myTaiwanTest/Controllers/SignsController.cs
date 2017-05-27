using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using myTaiwanTest.Models;

namespace myTaiwanTest.Controllers
{
    public class SignsController : Controller
    {
        private myTaiwanEntities db = new myTaiwanEntities();

        // GET: Signs
        public ActionResult Index()
        {
            return View(db.Signs.ToList());
        }

        // GET: Signs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sign sign = db.Signs.Find(id);
            if (sign == null)
            {
                return HttpNotFound();
            }
            return View(sign);
        }

        // GET: Signs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Signs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,passwords,email,faceUrl,coverUrl")] Sign sign)
        {
            if (ModelState.IsValid)
            {
                db.Signs.Add(sign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sign);
        }
        //註冊
        public ActionResult SignIn() {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "name,passwords,email")] Sign sign) {
            if (ModelState.IsValid) {
                sign.coverUrl = "";
                sign.faceUrl = "";
                db.Signs.Add(sign);
                try
                {
                    db.SaveChanges();
                    var query = db.Signs.FirstOrDefault(o => o.name == sign.name &&
                                        o.passwords == sign.passwords);
                    if (query != null) {
                        Session["userName"] = query.name;
                        Session["userID"] = query.ID;
                        return RedirectToAction("BrowseText");
                    }
                    
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            return View("Login", sign);
        }


        public ActionResult LogIn()
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            var BrowseText = db.sp_BrowseText(UserID);
            AddfriendPlusText mix = new AddfriendPlusText() {
                browseText = BrowseText,
                sign = sign
            };
            return View("DynamicIndex", mix);
        }
        //以使用者為主秀出文章
        public ActionResult BrowseText()
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            var BrowseText = db.sp_BrowseText(UserID);
            AddfriendPlusText mix = new AddfriendPlusText() {
                browseText = BrowseText,
                sign = sign
            };
            return View("DynamicIndex", mix);
        }
        //以使用者為主秀出文章(限定地區)
        public ActionResult BrowseTextByCounty(string id) {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            var countyID = db.Counties.First(c => c.countryName == id).countryID;
            var BrowseText = db.sp_BrowseTextbyCounty(UserID, countyID);
            AddfriendPlusText mix = new AddfriendPlusText() {
                browseTextbyCounty = BrowseText,
                sign = sign,
            };
            return View("DynamicIndex", mix);
        }

        //登入
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "name,passwords")] Sign sign)
        {
            if (ModelState.IsValid)
            {
                //var query = from o in db.Signs
                //            where o.name == sign.name &&
                //                  o.passwords == sign.passwords
                //            select o;
                var query = db.Signs.FirstOrDefault(o => o.name == sign.name && 
                                        o.passwords == sign.passwords);
                try
                {
                    if (query != null)
                    {
                        Session["userName"] = query.name;
                        Session["userID"] = query.ID;
                        return RedirectToAction("BrowseText");
                    }
                        
                }
                catch(Exception ex)
                {
                    ;
                }
                
            }
            return View("Login");
        }
        //登出
        public ActionResult LogOut()
        {
            //Session.Clear();
            return View("Login");
        }
        //好友列表
        public ActionResult myFriend()
        {
            return View();
        }
        //尋找好友(路人) //由searchtext取得資料
        [HttpPost]
        public ActionResult searchUser([Bind(Include = "name")] Sign signFriend)
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            AddfriendPlusText friendPage = new AddfriendPlusText();
            
            var friendTextDetail = db.Signs.FirstOrDefault(o => o.name == signFriend.name);//好友(路人)資訊
            friendPage.friendID = friendTextDetail.ID;
            friendPage.browseText = db.sp_BrowseText(friendTextDetail.ID);//文章資訊
            friendPage.FriendSign = friendTextDetail;
            friendPage.sign = sign;

            return View("FriendPage", friendPage);
        }
        //尋找好友 //由好友ID搜尋
        public ActionResult searchUser(int id) {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            AddfriendPlusText friendPage = new AddfriendPlusText();

            var friendTextDetail = db.Signs.FirstOrDefault(o => o.ID == id);//好友資訊
            friendPage.friendID = friendTextDetail.ID;
            friendPage.browseText = db.sp_BrowseText(friendTextDetail.ID);//文章資訊
            friendPage.FriendSign = friendTextDetail;
            friendPage.sign = sign;
            return View("FriendPage", friendPage);
        }
        //文章列表
        //public ActionResult ArctileIndex()
        //{
        //    return View();
        //}
        //新增好友
        public ActionResult addFriend(int id)
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            AddfriendPlusText friendPage = new AddfriendPlusText();

            Friend addFriend = new Friend() {
                userID = Convert.ToInt32(Session["userID"]),
                friendID = id
            };
            db.Friends.Add(addFriend);
            db.SaveChanges();
            var friendTextDetail = db.Signs.FirstOrDefault(o => o.ID == id);//好友(路人)資訊
            friendPage.friendID = friendTextDetail.ID;
            friendPage.browseText = db.sp_BrowseText(friendTextDetail.ID);//文章資訊
            friendPage.FriendSign = friendTextDetail;
            friendPage.sign = sign;

            return View("FriendPage", friendPage);
        }
        //刪除好友 
        public ActionResult delFriend(int id)
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == UserID);
            AddfriendPlusText friendPage = new AddfriendPlusText();
            var delfriend = db.Friends.FirstOrDefault(o => o.userID == UserID && o.friendID == id);
            db.Friends.Remove(delfriend);
            db.SaveChanges();
            var friendTextDetail = db.Signs.FirstOrDefault(o => o.ID == id);//好友(路人)資訊
            friendPage.friendID = friendTextDetail.ID;
            friendPage.browseText = db.sp_BrowseText(friendTextDetail.ID);//文章資訊
            friendPage.FriendSign = friendTextDetail;
            friendPage.sign = sign;

            return View("FriendPage", friendPage);
        }

        //public ActionResult addText()
        //{
        //    return View();
        //}

        public ActionResult singleText() {
            return View("SingleText");
        }

        //仁廷好友列表
        public ActionResult myFriends()
        {
            int UserID = Convert.ToInt32(Session["userID"]);
            var userName = Session["userName"].ToString();
            var query = db.Friends.Where(o => o.userID == UserID);

            var friend = from o in db.Signs
                         from f in query
                         where o.ID == f.friendID
                         select o;
            List<Sign> list = friend.ToList();
            Sign updateFace = db.Signs.FirstOrDefault(o => o.name == userName);
            FriendPlusDelFriend mix = new FriendPlusDelFriend()
            {
                sign = updateFace,
                friendList = list
            };
            return View("myFriends", mix);

        }
        public ActionResult delFriend1(int id)
        {
            var userName = Session["userName"].ToString();
            Friend list1 = db.Friends.FirstOrDefault(o => o.friendID == id);
            db.Friends.Remove(list1);
            db.SaveChanges();
            int UserID = Convert.ToInt32(Session["userID"]);

            var query = db.Friends.Where(o => o.userID == UserID);

            var friend = from o in db.Signs
                         from f in query
                         where o.ID == f.friendID
                         select o;
            List<Sign> list2 = friend.ToList();
            Sign updateFace = db.Signs.FirstOrDefault(o => o.name == userName);
            FriendPlusDelFriend mix = new FriendPlusDelFriend()
            {
                sign = updateFace,
                friendList = list2
            };
            return View("myFriends", mix);

        }

        public string forCity()
        {
            var a = from o in db.Counties
                    select o;

            List<County> city = a.ToList();
            List<cityModel> cityModel = new List<cityModel>();
            foreach (var ct in city)
            {
                cityModel.Add(new cityModel { countryID = ct.countryID, countryName = ct.countryName });
            }
            return JsonConvert.SerializeObject(cityModel);
        }
        public class cityModel
        {
            public int countryID { set; get; }
            public string countryName { set; get; }
        }

        //文章列表
        public ActionResult ArctileIndex()
        {
            var userName = Session["userName"].ToString();
            Sign updateFace = db.Signs.FirstOrDefault(o => o.name == userName);
            int UserID = Convert.ToInt32(Session["userID"]);

            var query = db.Texts.Where(o => o.userID == UserID);

            var newArctile = from o in query
                             join oo in db.Counties
                             on o.location equals oo.countryID
                             into ps
                             from oo in ps.DefaultIfEmpty()
                             select new locations
                             {

                                 txtID = o.txtID,
                                 txtTitle = o.txtTitle,
                                 txtText = o.txtText,
                                 txtCreateTime = o.txtCreateTime,
                                 city = oo.countryName

                             };
            List<locations> list = newArctile.ToList();
            FriendPlusDelFriend mix = new FriendPlusDelFriend()
            {
                sign = updateFace,
                locationInModel = list
            };

            return View("ArctileIndex", mix);
        }

        //從文章列表刪除文章
        public ActionResult delText(int id) {
            var text = db.Texts.Find(id);
            db.Texts.Remove(text);
            db.SaveChanges();

            return RedirectToAction("ArctileIndex");
        }

        //以下無用
        // GET: Signs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sign sign = db.Signs.Find(id);
            if (sign == null)
            {
                return HttpNotFound();
            }
            return View(sign);
        }

        // POST: Signs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,passwords,email,faceUrl,coverUrl")] Sign sign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sign);
        }

        // GET: Signs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sign sign = db.Signs.Find(id);
            if (sign == null)
            {
                return HttpNotFound();
            }
            return View(sign);
        }

        // POST: Signs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sign sign = db.Signs.Find(id);
            db.Signs.Remove(sign);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
