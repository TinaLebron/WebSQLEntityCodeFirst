using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.Services;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            var homeInfos = NewsServices.GetSubjectAndContents();
            ViewBag.Subject = homeInfos.Subject;
            ViewBag.HomeContents = homeInfos.Contents;

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
        public JsonResult GetNavbar(string logonId)
        {
            try
            {
                var personalPermissions = PermissionsServices.GetPersonalPermissions(logonId);

                return Json(new { navbars = personalPermissions });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }
        
    }
}