using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class NewsManagementController : Controller
    {
        // GET: NewsManagement
        public ActionResult Index()
        {
            return View();
        }
    }
}