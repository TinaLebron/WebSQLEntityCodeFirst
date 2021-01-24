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
    public class CourseManagementToTeacherController : Controller
    {
        // GET: CourseManagementToTeacher
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSectionDepartmentInitial(string courseSpecie, string status)
        {
            try
            {
                List<string> sectionList = new List<string>();
                SchoolContext db = new SchoolContext();
                List<CourseInQuiryInfos> courseInQuiryInfosList = new List<CourseInQuiryInfos>();
                var courses = db.Course.Where(x => x.IsActive == true).OrderByDescending(x => x.CourseID).ToList();
                int startingSchoolYear = courses[0].StartingSchoolYear;
                string schoolSemester = (courses[0].Semester == Core.Enums.Semester.F) ? "第一學期" : "第二學期";
                string loginId = Session["sIDNo"].ToString();

                if (courseSpecie == "系所課程")
                {
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }

                    courseInQuiryInfosList = GetCourseInQuiryInfosList(startingSchoolYear, schoolSemester, courseSpecie, "大學部", status, loginId);

                }
                else if (courseSpecie == "共同課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == "大學部").ToList();
                    var newSections = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G).Select(c => new { Section = c.Section }).Distinct().ToList();

                    foreach (var ns in newSections)
                    {
                        sectionList.Add(ns.Section);
                    }
                    
                    courseInQuiryInfosList = GetCourseInQuiryInfosList(startingSchoolYear, schoolSemester, courseSpecie, "大學部", status, loginId);

                }

                return Json(new { schoolYear = startingSchoolYear, semester = schoolSemester, sections = sectionList, courseInQuiryInfos = courseInQuiryInfosList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        public List<CourseInQuiryInfos> GetCourseInQuiryInfosList(int schoolYear, string semester, string courseSpecie, string section, string status, string loginId)
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
                && x.SectionDepartment.Section == section && x.IsActive == true && x.LogonId == loginId).OrderBy(x => x.CourseID).ToList();


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
                && x.SectionDepartment.Section == section && x.IsActive == true && x.LogonId == loginId).OrderBy(x => x.CourseID).ToList();


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
        public string GetStatusProgress(DateTime SignupBeginDate, DateTime SignupEndDate)
        {
            string status = "";

            if (DateTime.Now < SignupBeginDate)
            {
                status = "未開始";
            }
            else if (DateTime.Now > SignupBeginDate && DateTime.Now < SignupEndDate)
            {
                status = "進行中";
            }
            else if (DateTime.Now > SignupEndDate)
            {
                status = "已結束";
            }


            return status;

        }

        public string GetTimeLocation(int? classTimeId, int numberOfHours, string schoolNumber)
        {
            SchoolContext db = new SchoolContext();
            var classTimes1 = db.ClassTime.FirstOrDefault(x => x.ID == classTimeId);
            var week = classTimes1.Week;
            var classPeriod1 = classTimes1.ClassPeriod;
            var rank = classTimes1.Rank;


            var ClassPeriod2 = db.ClassTime.FirstOrDefault(x => x.Rank == (rank + numberOfHours) - 1).ClassPeriod;

            string timeLocation = "星期" + week + "(" + classPeriod1 + "節~" + ClassPeriod2 + "節) " + schoolNumber;

            return timeLocation;

        }

        public int GetCurrentNum(int coursesId)
        {
            SchoolContext db = new SchoolContext();
            var courseStatusCount = db.CourseStatus.Where(x => x.CourseId == coursesId).Count();
            if (courseStatusCount == 0) { return 0; }

            var courseStatus = db.CourseStatus.FirstOrDefault(x => x.CourseId == coursesId).CurrentQuota;


            return courseStatus;

        }

        [HttpPost]
        public JsonResult CourseSearch(int schoolYear, string semester, string courseSpecie, string section, string status)
        {
            try
            {
                string loginId = Session["sIDNo"].ToString();
                var courseInQuiryInfosList = GetCourseInQuiryInfosList(schoolYear, semester, courseSpecie, section, status, loginId);

                return Json(new { courseInQuiryInfos = courseInQuiryInfosList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }


        [HttpGet]
        public ActionResult EditCourse(int courseID)
        {
            SchoolContext db = new SchoolContext();

            var courses = db.Course.FirstOrDefault(x => x.CourseID == courseID);
            var SectionDepartmentId = courses.SectionDepartmentId;
            var sectionDepartments = db.SectionDepartment.FirstOrDefault(x => x.ID == SectionDepartmentId);
            
            var classTimes = db.ClassTime.FirstOrDefault(x => x.ID == courses.ClassTimeId);
            var week = classTimes.Week;
            var fTime = classTimes.ClassPeriod;
            var eTime = (classTimes.ClassPeriod + courses.NumberOfHours) - 1;
            var schoolNumber = "";

            if (courses.ClassroomId != null)
            {
                schoolNumber = db.Classroom.FirstOrDefault(x => x.ID == courses.ClassroomId).SchoolNumber;
            }
            
            
            ViewBag.editSchoolYear = courses.StartingSchoolYear;
            ViewBag.editSemester = (courses.Semester == 0) ? "第一學期" : "第二學期";
            ViewBag.editSubject = courses.Subject;
            ViewBag.editSubjectNumber = courses.SubjectNumber;
            ViewBag.editCourseDate = courses.CourseDate.Year + "/" + courses.CourseDate.Month + "/" + courses.CourseDate.Day;
            ViewBag.editSignupBeginDate = courses.SignupBeginDate.Year + "/" + courses.SignupBeginDate.Month + "/" + courses.SignupBeginDate.Day;
            ViewBag.editSignupEndDate = courses.SignupEndDate.Year + "/" + courses.SignupEndDate.Month + "/" + courses.SignupEndDate.Day;
            ViewBag.editMinNumber = courses.MinNumber;
            ViewBag.editMaxNumber = courses.MaxNumber;
            ViewBag.editCourseSort = (sectionDepartments.CourseSorts == 0) ? "系所課程" : "共同課程";
            if (sectionDepartments.CourseSorts == CourseSorts.C)
            {
                ViewBag.editSection = sectionDepartments.Section;
                ViewBag.editDepartment = sectionDepartments.Department;
                ViewBag.editGrade = courses.Grade;
                ViewBag.editClass = courses.Class;
                
            }
            else if (sectionDepartments.CourseSorts == CourseSorts.G)
            {
                ViewBag.editSection = sectionDepartments.Section;
                ViewBag.editDepartment = sectionDepartments.Department;
                ViewBag.editGrade = courses.Grade;
                ViewBag.editClass = courses.Class;
            }
            ViewBag.editUserName = courses.ApplicationUser.LogonId;
            ViewBag.editRequiredElective = (courses.RequiredElective == 0) ? "必修" : "選修";
            ViewBag.editCredits = courses.Credits;
            ViewBag.editNumberOfHours = courses.NumberOfHours;
            ViewBag.editHoursInternship = courses.HoursInternship;
            ViewBag.editWeek = week;
            ViewBag.editFTime = fTime;
            ViewBag.editETime = eTime;
            ViewBag.editSchoolNumber = schoolNumber;
            ViewBag.editObjectives = courses.CourseDescription.Objectives.Replace("<br />", "\n");
            ViewBag.editCourseOutline = courses.CourseDescription.CourseOutline.Replace("<br />", "\n");
            ViewBag.editTextbooks = courses.CourseDescription.Textbooks.Replace("<br />", "\n");
            ViewBag.editReferenceBooks = courses.CourseDescription.ReferenceBooks.Replace("<br />", "\n");
            ViewBag.editGrading = courses.CourseDescription.Grading.Replace("<br />", "\n");
            ViewBag.editSchedule = courses.CourseDescription.Schedule.Replace("<br />", "\n");

            ViewBag.courseID = courseID;

            return View();
        }

        [HttpPost]
        public JsonResult SaveEditInfo(string editObjectives, string editCourseOutline, string editTextbooks
            , string editReferenceBooks, string editGrading, string editSchedule, int courseID)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                
                var courseByEdit = db.Course.FirstOrDefault(x => x.CourseID == courseID);
                var courseDescriptionByEdit = db.CourseDescription.FirstOrDefault(x => x.ID == courseByEdit.CourseDescriptionId);
                courseDescriptionByEdit.Objectives = editObjectives.Replace("\n", "<br />");
                courseDescriptionByEdit.CourseOutline = editCourseOutline.Replace("\n", "<br />");
                courseDescriptionByEdit.Textbooks = editTextbooks.Replace("\n", "<br />");
                courseDescriptionByEdit.ReferenceBooks = editReferenceBooks.Replace("\n", "<br />");
                courseDescriptionByEdit.Grading = editGrading.Replace("\n", "<br />");
                courseDescriptionByEdit.Schedule = editSchedule.Replace("\n", "<br />");
                db.SaveChanges();
                

                return Json(new { message = "修改成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, errorInfo = "", error = false });
            }
        }

    }
}