using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using myTaiwanTest.Models;

namespace myTaiwanTest.Controllers {

    public class uploadImageController : Controller {
        private myTaiwanEntities db = new myTaiwanEntities();

        // GET: uploadImage
        public ActionResult ArctileIndex() {
            var userName = Session["userName"].ToString();
            int UserID = Convert.ToInt32(Session["userID"]);
            Sign updateFace = db.Signs.FirstOrDefault(o => o.name == userName);
            var query = db.Friends.Where(o => o.userID == UserID);

            var friend = from o in db.Signs
                         from f in query
                         where o.ID == f.friendID
                         select o;
            List<Sign> list = friend.ToList();
            FriendPlusDelFriend mix = new FriendPlusDelFriend() {
                sign = updateFace,
                friendList = list
            };
            return View("ArctileIndex", mix);
        }
        [HttpPost]
        public ActionResult uploadCover(HttpPostedFileBase imgOne) {
            var userName = Session["userName"].ToString();
            int UserID = Convert.ToInt32(Session["userID"]);
            if (imgOne != null && imgOne.ContentLength > 0) {
                
                var fileName = userName+".png";
                var path = Path.Combine(Server.MapPath("~/coverImage"), fileName);
                imgOne.SaveAs(path);
                db.sp_setCoverUrl("/coverImage/"+fileName,Convert.ToInt32(Session["userID"]));
            }
            Sign updateCover = db.Signs.FirstOrDefault(o => o.name == userName);
            var query = db.Friends.Where(o => o.userID == UserID);

            var friend = from o in db.Signs
                         from f in query
                         where o.ID == f.friendID
                         select o;
            List<Sign> list = friend.ToList();
            FriendPlusDelFriend mix = new FriendPlusDelFriend()
            {
                sign = updateCover,
                friendList = list
            };
            return Redirect("/signs/ArctileIndex");
        }
        [HttpPost]
        public ActionResult uploadfaceImage(HttpPostedFileBase imgTwo) {
            var userName = Session["userName"].ToString();
            int UserID = Convert.ToInt32(Session["userID"]);
            if (imgTwo != null && imgTwo.ContentLength > 0) {
                var fileName = userName+ ".png";
                var path = Path.Combine(Server.MapPath("~/faceImage"), fileName);
                imgTwo.SaveAs(path);
                db.sp_setFaceUrl("/faceImage/" + fileName, Convert.ToInt32(Session["userID"]));
            }
            Sign updateFace = db.Signs.FirstOrDefault(o => o.name == userName);
            var query = db.Friends.Where(o => o.userID == UserID);

            var friend = from o in db.Signs
                         from f in query
                         where o.ID == f.friendID
                         select o;
            List<Sign> list = friend.ToList();
            FriendPlusDelFriend mix = new FriendPlusDelFriend()
            {
                sign = updateFace,
                friendList = list
            };
            return Redirect("/signs/ArctileIndex");
        }



    }
}