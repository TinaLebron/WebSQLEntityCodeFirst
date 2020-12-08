using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SubjectSystem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Session["password"] == null)
            {
                Session["sIDNo"] = null;
                Session["password"] = null;
            }
            else
            {
                //登入為page1,首頁為page2
                Session["page"] = "page2";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult AccountsUpdateDialog(string sIDNo, string password, bool rememberMe)
        {
            try
            {
                


                return Json(new { url = Url.Action("Index", "UserInfo") });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }
    }
}