using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.Core.Enums;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class NotesOnCourseSelectionController : Controller
    {
        // GET: NotesOnCourseSelection
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CourseInQuiry()
        {

            return View();

        }

        [HttpGet]
        public ActionResult Syllabus(string yearSemesters, int courseID, int? applicationUserId, string gradeClass, string requiredElective, string timeLocation, string maximumNum)
        {

            SchoolContext db = new SchoolContext();
            var courses = db.Course.FirstOrDefault(x => x.CourseID == courseID);
            var applicationUsers = db.ApplicationUser.FirstOrDefault(x => x.ID == applicationUserId);
            var applicationUsers1 = db.ApplicationUser.FirstOrDefault(x => x.ID == applicationUserId && x.UserStateId == 1);
            var employees = db.Employee.FirstOrDefault(x => x.ApplicationUserId == applicationUserId);


            if (applicationUsers.ID != applicationUsers1.ID)
            {
                ViewBag.Email = "離職";
                ViewBag.SchoolNumber = "";
                ViewBag.Tel = "";
                ViewBag.ResearchAreas = "";
            }
            else
            {
                ViewBag.Email = (applicationUsers.Email == null) ? "" : applicationUsers.Email;
                ViewBag.SchoolNumber = (employees.Classroom.SchoolNumber == null) ? "" : employees.Classroom.Location + employees.Classroom.Floor + "(" + employees.Classroom.SchoolNumber + ")";
                ViewBag.Tel = (employees.Tel == null) ? "" : employees.Tel;
                ViewBag.ResearchAreas = (employees.ResearchAreas == null) ? "" : employees.ResearchAreas;
            }


            ViewBag.YearSemesters = yearSemesters;
            ViewBag.Subject = courses.Subject;
            ViewBag.SubjectNumber = courses.SubjectNumber;
            ViewBag.GradeClass = gradeClass;
            ViewBag.UserName = applicationUsers.UserName;
            ViewBag.RequiredElective = requiredElective;
            ViewBag.Credits = courses.Credits;
            ViewBag.NumberOfHours = courses.NumberOfHours;
            ViewBag.HoursInternship = courses.HoursInternship;
            ViewBag.TimeLocation = timeLocation;

            ViewBag.Objectives = courses.CourseDescription.Objectives;
            ViewBag.CourseOutline = courses.CourseDescription.CourseOutline;
            ViewBag.Textbooks = courses.CourseDescription.Textbooks;
            ViewBag.ReferenceBooks = courses.CourseDescription.ReferenceBooks;
            ViewBag.Grading = courses.CourseDescription.Grading;
            ViewBag.Schedule = courses.CourseDescription.Schedule;


            return View();

        }

        [HttpGet]
        public ActionResult CourseTimeQuiry()
        {
            SchoolContext db = new SchoolContext();
            List<CourseTimeQuiryDto> getCourseTimePerSemesterList = new List<CourseTimeQuiryDto>();
            var courseTimePerSemester = db.CourseTimePerSemester.OrderByDescending(x=>x.ID).ToList();
            var startingSchoolYear = courseTimePerSemester[0].StartingSchoolYear;
            var semester = courseTimePerSemester[0].Semester;
            var signupEndDate = courseTimePerSemester[0].SignupEndDate.ToString("yyyy/MM/dd HH:mm");
            var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();
            var CourseTimePerSemester1 = db.CourseTimePerSemester.Where(x=>x.StartingSchoolYear == startingSchoolYear && x.Semester == semester);
            

            foreach (var ns in newSections)
            {
                CourseTimeQuiryDto CourseTimeQuiryDto = new CourseTimeQuiryDto();
                List<CourseTime> courseTimeList = new List<CourseTime>();
                CourseTimeQuiryDto.SchoolSystem = ns.Section;

                foreach (var c in CourseTimePerSemester1)
                {
                    if (ns.Section == c.Section)
                    {
                        CourseTimeQuiryDto.CourseTimePerSemesterID = c.ID;
                        CourseTime courseTime = new CourseTime();
                        courseTime.CourseTimeString = (c.Grade > 3) ? c.Grade + "年級(含以上) " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm") : c.Grade + "年級 " + c.SignupBeginDate.ToString("yyyy/MM/dd HH:mm");
                        courseTimeList.Add(courseTime);
                    }
                    
                    
                }
                CourseTimeQuiryDto.CourseTimeList = courseTimeList;

                getCourseTimePerSemesterList.Add(CourseTimeQuiryDto);
            }
            

            ViewBag.editSchoolYear = startingSchoolYear;
            ViewBag.semester = (semester == Semester.F) ? "第一學期" : "第二學期";
            ViewBag.SignupEndDate = signupEndDate;
            ViewBag.editSections = getCourseTimePerSemesterList;


            return View();
        }

    }
}