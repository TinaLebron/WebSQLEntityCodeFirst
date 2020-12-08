using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.Services;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.Core.DataModels;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;



namespace WebSQLEntityCodeFirst.Controllers
{
    public class HomeController : Controller
    {
        //private SchoolContext db = new SchoolContext();

        public ActionResult Index()
        {
            //-----------  -------------    ---------
            //練習1
            //var studentList = db.Student.Where(x=>x.Name == "周O倫").ToList();
            //ViewBag.Message = studentList[0].Name.ToString();

            //練習2
            // 定義Student是來源的Class而StudentDto是最後結果
            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<Student, StudentDto>();
            //});
            //IMapper mapper = config.CreateMapper();
            // 把List<Student>轉成List<StudentDtol>
            //var viewModel2 = mapper.Map<List<StudentDto>>(db.Student.ToList());
            //var studentList = viewModel2.Where(x => x.Name == "周O倫").ToList();
            //ViewBag.Message = studentList[0].Name.ToString();
            //-----------  -------------    ---------

            if (Session["page"] != null && Session["page"].ToString() == "page2")
            {
                var rememberMeModel = new ApplicationUser()
                {
                    LogonId = Session["sIDNo"].ToString(),
                    PasswordHash = Session["password"].ToString()
                };

                Session["sIDNo"] = null;
                Session["password"] = null;
                Session["page"] = null;

                // 傳出ViewModel至頁面
                return View(rememberMeModel);
            }
            else if (Session["sIDNo"] != null)
            {
                return RedirectToAction("../News/Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult AccountLogin(string sIDNo, string password, bool rememberMe)
        {
            try
            {
                bool islogin = ApplicationUserServices.IsLoginBySIDNoAndPassword(sIDNo, password);
                if (!islogin) { throw new Exception("請確認帳號密碼是否正確!!!"); }

                string ESPMessage = ApplicationUserServices.ExcludeSpecialPersons(sIDNo);
                if (ESPMessage != null) { throw new Exception(ESPMessage); }

                Session["sIDNo"] = sIDNo;

                if (rememberMe)
                {
                    Session["password"] = password;
                    //登入為page1,首頁為page2
                    Session["page"] = "page1";

                }



                return Json(new { url = Url.Action("Index", "News") });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult ConfirmIdentity(string iDNo)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                var userCount = db.ApplicationUser.Where(x => x.IDNo == iDNo).Count();
                if (userCount == 0){ throw new Exception("輸入有誤,如有問題請洽詢(0x)1111111"); }

                var user = db.ApplicationUser.FirstOrDefault(x => x.IDNo == iDNo);
                string userLogonId = user.LogonId;

                return Json(new { logonId = userLogonId });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult SendEmail(string receiver)
        {
            try
            {

                var senderEmail = new MailAddress("", "xxx學校行政單位");
                var receiverEmail = new MailAddress(receiver, "Receiver");
                var password = "";//請先在google做smtp設定,後會有密碼
                var sub = "學生系統驗證碼";

                Random Rnd = new Random(); //加入Random，產生的數字不會重覆
                int randomNum = Rnd.Next(10001, 100000);
                var body = "驗證碼:" + randomNum;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
                return Json(new { num = randomNum });
            }
            catch (Exception ex)
            {

                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpGet]
        public ActionResult GetNewPassword(string logonId, string password)
        {
            
            ApplicationUserServices.UserpasswordUpdate(logonId, password);

            return RedirectToAction("Index", "Home");

        }
        
    }
}