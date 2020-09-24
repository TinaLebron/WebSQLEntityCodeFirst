using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.Core.DataModels;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public ActionResult Index()
        {
            //var studentList = db.Student.Where(x=>x.Name == "周O倫").ToList();
            //ViewBag.Message = studentList[0].Name.ToString();

            
            

            // 定義Student是來源的Class而StudentDto是最後結果
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Student, StudentDto>();
            });
            IMapper mapper = config.CreateMapper();
            // 把List<Student>轉成List<StudentDtol>
            var viewModel2 = mapper.Map<List<StudentDto>>(db.Student.ToList());
            var studentList = viewModel2.Where(x => x.Name == "周O倫").ToList();
            ViewBag.Message = studentList[0].Name.ToString();

            return View();
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


        
    }
}