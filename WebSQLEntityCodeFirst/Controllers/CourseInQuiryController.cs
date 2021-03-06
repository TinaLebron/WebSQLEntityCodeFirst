﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.Core.Enums;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Controllers
{
    public class CourseInQuiryController : Controller
    {
        // GET: CourseInQuiry
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Syllabus()
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


        [HttpPost]
        public JsonResult GetSectionDepartmentInitial(string courseSpecie, string status)
        {
            try
            {
                List<string> sectionList = new List<string>();
                List<string> sectionDepartmentCoreList = new List<string>();
                SchoolContext db = new SchoolContext();
                List<CourseInQuiryInfos> courseInQuiryInfosList = new List<CourseInQuiryInfos>();
                var courses = db.Course.Where(x=>x.IsActive==true).OrderByDescending(x=>x.CourseID).ToList();
                int startingSchoolYear = courses[0].StartingSchoolYear;
                string schoolSemester = (courses[0].Semester==Core.Enums.Semester.F)? "第一學期" : "第二學期";

                if (courseSpecie == "系所課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == "大學部").ToList();
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }

                    courseInQuiryInfosList = GetCourseInQuiryInfosList(startingSchoolYear, schoolSemester, courseSpecie, "大學部", sectionDepartmentCores[0].Department, 1, "A", status);

                }
                else if (courseSpecie == "共同課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == "大學部").ToList();
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }

                    courseInQuiryInfosList = GetCourseInQuiryInfosList(startingSchoolYear, schoolSemester, courseSpecie, "大學部", sectionDepartmentCores[0].Department, 1, null, status);

                }

                return Json(new { schoolYear = startingSchoolYear, semester = schoolSemester, sections = sectionList, sectionDepartmentCores = sectionDepartmentCoreList, courseInQuiryInfos = courseInQuiryInfosList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult GetSectionDepartment(string courseSpecie)
        {
            try
            {
                List<string> sectionList = new List<string>();
                List<string> sectionDepartmentCoreList = new List<string>();
                SchoolContext db = new SchoolContext();

                if (courseSpecie == "系所課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == "大學部").ToList();
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }

                }
                else if (courseSpecie == "共同課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == "大學部").ToList();
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }
                }

                return Json(new { sections = sectionList, sectionDepartmentCores = sectionDepartmentCoreList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult GetSections(string courseSpecie,string section)
        {
            try
            {
                List<string> sectionDepartmentCoreList = new List<string>();
                SchoolContext db = new SchoolContext();
                
                if (courseSpecie == "系所課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == section).ToList();
                    

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }
                }
                else if(courseSpecie == "共同課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == section).ToList();
                    

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }
                }

                return Json(new { sectionDepartmentCores = sectionDepartmentCoreList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult CourseSearch(int schoolYear,string semester,string courseSpecie,string section,string department,int grade,string class1,string status)
        {
            try
            {
                var courseInQuiryInfosList = GetCourseInQuiryInfosList(schoolYear, semester, courseSpecie, section, department, grade, class1, status);

                return Json(new { courseInQuiryInfos = courseInQuiryInfosList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        public List<CourseInQuiryInfos> GetCourseInQuiryInfosList(int schoolYear, string semester, string courseSpecie, string section, string department, int grade, string class1, string status)
        {
            List<CourseInQuiryInfos> courseInQuiryInfosList = new List<CourseInQuiryInfos>();
            SchoolContext db = new SchoolContext();
            Semester semester1 = Semester.F;

            if (semester == "第一學期")
            {
                semester1 = Semester.F;
            }
            else if (semester == "第二學期")
            {
                semester1 = Semester.S;
            }


            if (courseSpecie == "系所課程")
            {
                var Courses = db.Course.Where(x => x.StartingSchoolYear == schoolYear && x.Semester == semester1 && x.SectionDepartment.CourseSorts == CourseSorts.C
                && x.SectionDepartment.Section == section && x.SectionDepartment.Department == department && x.Grade == grade && x.Class == class1 && x.IsActive == true).OrderBy(x => x.CourseID).ToList();


                foreach (var c in Courses)
                {
                    string requiredElective = "";
                    string status1 = GetStatusProgress(c.SignupBeginDate, c.SignupEndDate);

                    if (c.RequiredElective == RequiredElective.E)
                    {
                        requiredElective = "選";
                    }
                    else if (c.RequiredElective == RequiredElective.R)
                    {
                        requiredElective = "必";
                    }

                    string schoolNumber = "";

                    if (c.ClassroomId != null)
                    {
                        schoolNumber = c.Classroom.SchoolNumber;
                    }

                    if (status == status1 || status == null)
                    {
                        CourseInQuiryInfos courseInQuiryInfos = new CourseInQuiryInfos
                        {
                            CourseID = c.CourseID,
                            Subject = c.Subject,
                            SubjectNumber = c.SubjectNumber,
                            Credits = c.Credits,
                            ClassYear = schoolYear + " " + semester,
                            GradeClass = c.SectionDepartment.Section + c.SectionDepartment.DepartmentAbbreviation + c.Grade + c.Class,
                            RequiredElective = requiredElective,
                            TimeLocation = GetTimeLocation(c.ClassTimeId, c.NumberOfHours, schoolNumber),
                            Instructor = c.ApplicationUser.UserName,
                            MaximumNum = c.MinNumber + "-" + c.MaxNumber,
                            CurrentNum = GetCurrentNum(c.CourseID),
                            ApplicationUserId = c.ApplicationUserId,
                            Status = status1
                        };
                        courseInQuiryInfosList.Add(courseInQuiryInfos);
                    }
                    
                    

                    
                }
            }
            else if (courseSpecie == "共同課程")
            {
                var Courses = db.Course.Where(x => x.StartingSchoolYear == schoolYear && x.Semester == semester1 && x.SectionDepartment.CourseSorts == CourseSorts.G
                && x.SectionDepartment.Section == section && x.SectionDepartment.Department == department && x.Grade == grade && x.IsActive == true).OrderBy(x => x.CourseID).ToList();


                foreach (var c in Courses)
                {
                    string requiredElective = "";
                    string status1 = GetStatusProgress(c.SignupBeginDate, c.SignupEndDate);

                    if (c.RequiredElective == RequiredElective.E)
                    {
                        requiredElective = "選";
                    }
                    else if (c.RequiredElective == RequiredElective.R)
                    {
                        requiredElective = "必";
                    }

                    string schoolNumber = "";

                    if (c.ClassroomId != null)
                    {
                        schoolNumber = c.Classroom.SchoolNumber;
                    }

                    if (status == status1 || status == null)
                    {
                        CourseInQuiryInfos courseInQuiryInfos = new CourseInQuiryInfos
                        {
                            CourseID = c.CourseID,
                            Subject = c.Subject,
                            SubjectNumber = c.SubjectNumber,
                            Credits = c.Credits,
                            ClassYear = schoolYear + " " + semester,
                            GradeClass = c.SectionDepartment.Section + c.SectionDepartment.DepartmentAbbreviation + c.Grade,
                            RequiredElective = requiredElective,
                            TimeLocation = GetTimeLocation(c.ClassTimeId, c.NumberOfHours, schoolNumber),
                            Instructor = c.ApplicationUser.UserName,
                            MaximumNum = c.MinNumber + "-" + c.MaxNumber,
                            CurrentNum = GetCurrentNum(c.CourseID),
                            ApplicationUserId = c.ApplicationUserId,
                            Status = status1
                        };


                        courseInQuiryInfosList.Add(courseInQuiryInfos);
                    }

                    
                }
            }

            return courseInQuiryInfosList;

        }


        public string GetTimeLocation(int? classTimeId,int numberOfHours, string schoolNumber)
        {
            SchoolContext db = new SchoolContext();
            var classTimes1 = db.ClassTime.FirstOrDefault(x => x.ID == classTimeId);
            var week = classTimes1.Week;
            var classPeriod1 = classTimes1.ClassPeriod;
            var rank = classTimes1.Rank;
            

            var ClassPeriod2 = db.ClassTime.FirstOrDefault(x => x.Rank == (rank + numberOfHours)-1).ClassPeriod;

            string timeLocation = "星期" + week + "(" + classPeriod1 + "節~" + ClassPeriod2 + "節) " + schoolNumber;

            return timeLocation;

        }

        public int GetCurrentNum(int coursesId)
        {
            SchoolContext db = new SchoolContext();
            var courseStatusCount = db.CourseStatus.Where(x => x.CourseId == coursesId).Count();
            if (courseStatusCount == 0 ) { return 0; }

            var courseStatus = db.CourseStatus.FirstOrDefault(x => x.CourseId == coursesId).CurrentQuota;
            

            return courseStatus;

        }

        public string GetStatusProgress(DateTime SignupBeginDate, DateTime SignupEndDate)
        {
            string status = "";

            if (DateTime.Now < SignupBeginDate)
            {
                status = "未開始";
            }
            else if(DateTime.Now > SignupBeginDate && DateTime.Now < SignupEndDate)
            {
                status = "進行中";
            }
            else if (DateTime.Now > SignupEndDate)
            {
                status = "已結束";
            }


            return status;

        }

       

    }
}