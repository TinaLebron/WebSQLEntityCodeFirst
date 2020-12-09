using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.Services;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class NewsManagementController : Controller
    {
        // GET: NewsManagement
        public ActionResult Index()
        {
            var homeInfos = NewsServices.GetSubjectAndContents();
            ViewBag.Subject = homeInfos.Subject;
            ViewBag.HomeContents = homeInfos.Contents;

            return View();
        }
        
        [HttpPost]
        public ActionResult SaveHomeInfo(string subject, string contents)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string logonId = Session["sIDNo"].ToString();
                var user = db.ApplicationUser.FirstOrDefault(x => x.LogonId == logonId);
                int userId = user.ID;

                NewsServices.UpdateHomeInfo(subject, contents, userId);

                return Json(new { resp = "" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

    }
}