using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.Services;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class PasswordSettingController : Controller
    {
        // GET: SendEmail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PasswordUpdate(string password)
        {
            try
            {
                ApplicationUserServices.UserpasswordUpdate(Session["sIDNo"].ToString(), password);

                return Json(new { successMessage = "密碼已更新!" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        
    }
}