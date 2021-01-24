using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.Core.DataModels;
using WebSQLEntityCodeFirst.Core.Enums;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class CourseTimePerSemesterController : Controller
    {
        // GET: CourseTimePerSemester
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCourseTimePerSemester()
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<GetCourseTimePerSemester> getCourseTimePerSemesterList = new List<GetCourseTimePerSemester>();

                List<string> sectionList = new List<string>();
                var section = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();
                foreach (var s in section)
                {
                    sectionList.Add(s.Section);
                }

                var courses = db.Course.Where(x => x.IsActive == true).OrderByDescending(x => x.CourseID).ToList();
                var startingSchoolYear = courses[0].StartingSchoolYear;
                var schoolsemester = courses[0].Semester;
                var schoolsemesterString = (courses[0].Semester == Semester.F)? "第一學期" : "第二學期";
                var courseTimePerSemester = db.CourseTimePerSemester.Where(x=>x.StartingSchoolYear == startingSchoolYear && x.Semester == schoolsemester).ToList();

                foreach (var c in courseTimePerSemester)
                {
                    var semesterString = (c.Semester == Semester.F)? "第一學期" : "第二學期";
                    GetCourseTimePerSemester getCourseTimePerSemester = new GetCourseTimePerSemester();
                    getCourseTimePerSemester.CourseTimePerSemesterID = c.ID;
                    getCourseTimePerSemester.SchoolYearSemester = c.StartingSchoolYear + "學年度" + semesterString;
                    getCourseTimePerSemester.SchoolSystem = c.Section;
                    getCourseTimePerSemester.CourseTime = (c.Grade > 3)? c.Grade + "年級(含以上) " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm") + " ~ " + c.SignupEndDate.ToString("yyyy/MM/dd HH:mm") : c.Grade + "年級 " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm") + " ~ " + c.SignupEndDate.ToString("yyyy/MM/dd HH:mm");
                    
                    getCourseTimePerSemesterList.Add(getCourseTimePerSemester);

                }




                return Json(new { schoolYear = startingSchoolYear, semester = schoolsemesterString, sections = sectionList, courseTimePerSemester = getCourseTimePerSemesterList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult CourseTimePerSemesterSearch(int schoolYear, string semester)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<GetCourseTimePerSemester> getCourseTimePerSemesterList = new List<GetCourseTimePerSemester>();
                var schoolsemester = (semester == "第一學期") ? Semester.F : Semester.S;
                var courseTimePerSemester = db.CourseTimePerSemester.Where(x => x.StartingSchoolYear == schoolYear && x.Semester == schoolsemester).ToList();

                foreach (var c in courseTimePerSemester)
                {
                    var semesterString = (c.Semester == Semester.F) ? "第一學期" : "第二學期";
                    GetCourseTimePerSemester getCourseTimePerSemester = new GetCourseTimePerSemester();
                    getCourseTimePerSemester.CourseTimePerSemesterID = c.ID;
                    getCourseTimePerSemester.SchoolYearSemester = c.StartingSchoolYear + "學年度" + semesterString;
                    getCourseTimePerSemester.SchoolSystem = c.Section;
                    getCourseTimePerSemester.CourseTime = (c.Grade > 3) ? c.Grade + "年級(含以上) " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm") + " ~ " + c.SignupEndDate.ToString("yyyy/MM/dd HH:mm") : c.Grade + "年級 " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm") + " ~ " + c.SignupEndDate.ToString("yyyy/MM/dd HH:mm");

                    getCourseTimePerSemesterList.Add(getCourseTimePerSemester);

                }


                return Json(new { courseTimePerSemester = getCourseTimePerSemesterList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult SaveAddInfo(int addSchoolYear, string addSemester,DateTime addSignupBeginDate, DateTime addSignupEndDate,string addSection, int addGrade)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;

                CourseTimePerSemester CourseTimePerSemesterDto = new CourseTimePerSemester();
                CourseTimePerSemesterDto.StartingSchoolYear = addSchoolYear;
                CourseTimePerSemesterDto.Semester = (addSemester == "第一學期")? Semester.F: Semester.S;
                CourseTimePerSemesterDto.Grade = addGrade;
                CourseTimePerSemesterDto.SignupBeginDate = addSignupBeginDate;
                CourseTimePerSemesterDto.SignupEndDate = addSignupEndDate;
                CourseTimePerSemesterDto.CreatedUserId = createdUserId;
                CourseTimePerSemesterDto.CreateDate = DateTime.Now;
                CourseTimePerSemesterDto.Section = addSection;
                db.CourseTimePerSemester.Add(CourseTimePerSemesterDto);
                db.SaveChanges();



                return Json(new { message = "新增成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpGet]
        public ActionResult EditCourseTime(int courseTimePerSemesterID)
        {
            SchoolContext db = new SchoolContext();

            List<string> sectionList = new List<string>();
            var section = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();
            foreach (var s in section)
            {
                sectionList.Add(s.Section);
            }

            var courseTimePerSemester = db.CourseTimePerSemester.FirstOrDefault(x=>x.ID == courseTimePerSemesterID);

            ViewBag.courseTimePerSemesterID = courseTimePerSemesterID;

            ViewBag.editSchoolYear = courseTimePerSemester.StartingSchoolYear;
            ViewBag.editSemester = (courseTimePerSemester.Semester== Semester.F)? "第一學期" : "第二學期";
            ViewBag.editSignupBeginDate = courseTimePerSemester.SignupBeginDate.ToString("yyyy/MM/dd HH:mm");
            ViewBag.editSignupEndDate = courseTimePerSemester.SignupEndDate.ToString("yyyy/MM/dd HH:mm");
            ViewBag.editSections = sectionList;
            ViewBag.editSection = courseTimePerSemester.Section;
            ViewBag.editGrade = courseTimePerSemester.Grade;

            return View(); 
        }

        [HttpPost]
        public JsonResult SaveEditInfo(int courseTimePerSemesterID,int editSchoolYear, string editSemester, DateTime editSignupBeginDate, DateTime editSignupEndDate, string editSection, int editGrade)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;

                var courseTimePerSemester = db.CourseTimePerSemester.FirstOrDefault(x=>x.ID == courseTimePerSemesterID);
                courseTimePerSemester.StartingSchoolYear = editSchoolYear;
                courseTimePerSemester.Semester = (editSemester == "第一學期") ? Semester.F : Semester.S;
                courseTimePerSemester.Grade = editGrade;
                courseTimePerSemester.SignupBeginDate = editSignupBeginDate;
                courseTimePerSemester.SignupEndDate = editSignupEndDate;
                courseTimePerSemester.LastModifiedUserId = createdUserId;
                courseTimePerSemester.LastModifyDate = DateTime.Now;
                courseTimePerSemester.Section = editSection;
                db.SaveChanges();



                return Json(new { message = "新增成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpGet]
        public ActionResult DeleteCourseTime(int courseTimePerSemesterID)
        {
            ViewBag.courseTimePerSemesterID = courseTimePerSemesterID;
            return View();
        }

        [HttpPost]
        public JsonResult SaveDeleteCourseTime(int courseTimePerSemesterID)
        {
            try
            {
                SchoolContext db = new SchoolContext();

                var courseTimePerSemester = db.CourseTimePerSemester.FirstOrDefault(x => x.ID == courseTimePerSemesterID);
                db.CourseTimePerSemester.Remove(courseTimePerSemester);
                db.SaveChanges();



                return Json(new { message = "刪除成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

    }
}