using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTaiwanTest.Models;
using System.IO;

namespace myTaiwanTest.Controllers
{
    public class addController : Controller
    {
        private myTaiwanEntities db = new myTaiwanEntities();

        // GET: add/addTest
        public ActionResult addTest() {
            var userID = Convert.ToInt32(Session["userID"]);
            var sign = db.Signs.FirstOrDefault(o => o.ID == userID);
            AddEditTextModel mix = new AddEditTextModel() {
                sign = sign
            };
            return View(mix);
        }
        
        /// <summary>
        /// ckeditor上傳圖片
        /// </summary>
        /// <param name="upload">預設參數叫upload</param>
        /// <param name="CKEditorFuncNum"></param>
        /// <param name="CKEditor"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [HttpPost]
        // GET: add/UploadPicture
        public ActionResult UploadPicture(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode) {
            string result = "";
            if (upload != null && upload.ContentLength > 0) {
                //上傳儲存圖片至Server(路徑：~/addTestImages/)
                upload.SaveAs(Server.MapPath("~/addTestImages/" + upload.FileName));

                //取得預覽圖片於Server上(路徑：~/addTestImages/)
                var imageUrl = Url.Content("~/addTestImages/" + upload.FileName);

                var vMessage = string.Empty;

                result = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + imageUrl + "\", \"" + vMessage + "\");</script></body></html>";
            }

            return Content(result);
        }
        public class myPostObj
        {
            public string txtTitle { set; get; }
            public string txtText { set; get; }
            public string location { set; get; }
            public string locationDescription { set; get; }
            public HttpPostedFileBase imgOne { set; get; }
        }
        [HttpPost]
        public string UploadText(myPostObj obj)
        {
            Text text = new Text();
            text.txtTitle = obj.txtTitle;
            text.txtText = obj.txtText;
            text.txtCreateTime = DateTime.Now;
            text.txtUpdateTime = DateTime.Now;
            text.userID = Convert.ToInt32(Session["userID"]);
            var county = db.Counties.FirstOrDefault(o => o.countryName == obj.location);
            if (county != null){
                text.location = county.countryID;
            }
            else {
                text.location = 0;
            }
            text.locationDescription = "";
            db.Texts.Add(text);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "false";
            }

            return "true";
        }

        [HttpPost]
        public ActionResult uploadImage(HttpPostedFileBase imgOne) {
            var userID = Convert.ToInt32(Session["userID"]);
            var userName = Session["userName"].ToString();
            var sign = db.Signs.FirstOrDefault(o => o.ID == userID);
            //因為Linq不支援Last語法，所以需要先轉成物件(使用toList())之後，才能使用Last()
            var nextText = (from o in db.Texts
                            select o.txtID).ToList().Last();//因為還沒新增文章，所以找到文章列表的最後一份文章ID，然後加一
            
            AddEditTextModel mix = new AddEditTextModel() {
                sign = sign
            };
            
            if (imgOne != null && imgOne.ContentLength > 0) {

                var fileName = userName + "-" + nextText + ".png";//Ex:Milktea123-11.png
                var path = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                imgOne.SaveAs(path);
                Picture picture = new Picture();
                picture.picUrl = "/Pictures/" + fileName;
                picture.txtID = nextText;
                picture.location = 0;
                picture.picDescription = "";
                db.Pictures.Add(picture);
                db.SaveChanges();
            }

            return Redirect("/signs/BrowseText");
        }
    }
}