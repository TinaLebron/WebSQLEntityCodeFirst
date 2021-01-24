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
    public class CourseWorkController : Controller
    {
        // GET: CourseWork
        public ActionResult Index()
        {
            SchoolContext db = new SchoolContext();
            bool isOpen = false;
            if (Session["sIDNo"]==null) { return View(); }
            string loginId = Session["sIDNo"].ToString();
            var user = db.Student.FirstOrDefault(x => x.LogonId == loginId);
            var section = user.SectionDepartment.Section;
            var grade = (user.Grade>3)? 4: user.Grade;
            var courseTimePerSemester = db.CourseTimePerSemester.Where(x => x.SignupBeginDate <= DateTime.Now && DateTime.Now <= x.SignupEndDate && x.Section == section && x.Grade == grade);
            if (courseTimePerSemester.Count() != 0)
            {
                isOpen = true;
            }

            ViewBag.isOpen = isOpen;

            return View();
        }

        [HttpPost]
        public JsonResult GetCourseList()
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                List<CourseListsByCourseWork> courseListsByCourseWorkList = new List<CourseListsByCourseWork>();
                List<CourseListsByCartsByCourseWork> courseListsByCartsByCourseWorkList = new List<CourseListsByCartsByCourseWork>();
                List<CourseListsByCartsByCourseWork> courseListsByCartsByCourseWorkList1 = new List<CourseListsByCartsByCourseWork>(); //幫前端的courseListsByCarts初始化

                //搜尋條件-----
                List<string> departmentList = new List<string>();
                var user = db.Student.FirstOrDefault(x=>x.LogonId == loginId); 
                var userSection = user.SectionDepartment.Section;
                var sectionDepartment = user.SectionDepartment.Department;
                var Grade = (user.Grade > 3) ? 4 : user.Grade;
                var Class = user.Class;
                var sectionDepartments = db.SectionDepartment.Where(x=>x.CourseSorts == CourseSorts.C && x.Section == userSection);
                foreach (var sd in sectionDepartments)
                {
                    departmentList.Add(sd.Department);
                }
                //----------------

                //課程清單------------
                var Courses = db.Course.Where(x=>x.IsActive == true && x.SignupBeginDate <= DateTime.Now && DateTime.Now <= x.SignupEndDate && x.SectionDepartmentId == user.SectionDepartmentId && x.Grade == Grade && x.Class == user.Class).OrderBy(x=>x.CourseID).ToList();
                foreach (var c in Courses)
                {
                    var eTimePeriod = (c.ClassTime.ClassPeriod + c.NumberOfHours) - 1;
                    var schoolNumber = (c.Classroom == null)?"不指定教室": c.Classroom.SchoolNumber;
                    CourseListsByCourseWork courseListsByCourseWork = new CourseListsByCourseWork();
                    courseListsByCourseWork.CourseId = c.CourseID;
                    courseListsByCourseWork.SubjectNumber = c.SubjectNumber;
                    courseListsByCourseWork.Subject = c.Subject;
                    courseListsByCourseWork.Credits = c.Credits;
                    courseListsByCourseWork.TimeLocation = "星期" + c.ClassTime.Week + "(" + c.ClassTime.ClassPeriod + "節~" + eTimePeriod + "節) " + schoolNumber; //ex: 星期一(2節~4節) T20101
                    courseListsByCourseWork.IsCourseLists = false;
                    courseListsByCourseWork.Week = c.ClassTime.Week;
                    courseListsByCourseWork.FTime = c.ClassTime.ClassPeriod;
                    courseListsByCourseWork.ETime = eTimePeriod;

                    courseListsByCourseWorkList.Add(courseListsByCourseWork);
                }
                //---------------------------

                //我的課程購物車----------------------
                var courses = db.Course.Where(x => x.IsActive == true).OrderByDescending(x => x.CourseID).ToList();
                var startingSchoolYear = courses[0].StartingSchoolYear;
                var semester = courses[0].Semester;
                var electives = db.Elective.Where(x=>x.StartingSchoolYear == startingSchoolYear && x.Semester == semester && x.LogonId == loginId).ToList();
                foreach (var e in electives)
                {
                    var eTimePeriod = (e.Course.ClassTime.ClassPeriod + e.Course.NumberOfHours) - 1;
                    var schoolNumber = (e.Course.Classroom == null) ? "不指定教室" : e.Course.Classroom.SchoolNumber;
                    CourseListsByCartsByCourseWork courseListsByCartsByCourseWork = new CourseListsByCartsByCourseWork();
                    courseListsByCartsByCourseWork.ElectiveId = e.ID;
                    courseListsByCartsByCourseWork.CourseId = e.CourseId;
                    courseListsByCartsByCourseWork.SubjectNumber = e.Course.SubjectNumber;
                    courseListsByCartsByCourseWork.Subject = e.Course.Subject;
                    courseListsByCartsByCourseWork.Credits = e.Credits;
                    courseListsByCartsByCourseWork.TimeLocation = "星期" + e.Course.ClassTime.Week + "(" + e.Course.ClassTime.ClassPeriod + "節~" + eTimePeriod + "節) " + schoolNumber; //ex: 星期一(2節~4節) T20101
                    courseListsByCartsByCourseWork.Status = "選課成功";
                    courseListsByCartsByCourseWork.Week = e.Course.ClassTime.Week;
                    courseListsByCartsByCourseWork.FTime = e.Course.ClassTime.ClassPeriod;
                    courseListsByCartsByCourseWork.ETime = eTimePeriod;

                    courseListsByCartsByCourseWorkList.Add(courseListsByCartsByCourseWork);
                }

                //----------------------------

                return Json(new { sectionDepartmentCores = departmentList, sectionDepartmentCore = sectionDepartment, grade1 = Grade, class1 = Class, courseLists = courseListsByCourseWorkList, courseListsByCartOks = courseListsByCartsByCourseWorkList, courseListsByCarts = courseListsByCartsByCourseWorkList1 });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult GetDepartmentsByUserLogonId(string courseSpecie)
        {
            try
            {
                List<string> sectionDepartmentCoreList = new List<string>();
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var student = db.Student.FirstOrDefault(x=>x.LogonId == loginId);
                var section = student.SectionDepartment.Section;

                if (courseSpecie == "系所課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == section).ToList();
                    

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }

                }
                else if (courseSpecie == "共同課程")
                {
                    var sectionDepartmentCores = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == section).ToList();

                    foreach (var ns in sectionDepartmentCores)
                    {
                        sectionDepartmentCoreList.Add(ns.Department);
                    }
                }

                return Json(new { sectionDepartmentCores = sectionDepartmentCoreList, userDepartment = student.SectionDepartment.Department, userGrade = student.Grade, userClass = student.Class });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult SubjectNumberSearch(string courseNumber)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<CourseListsByCourseWork> courseListsByCourseWorkList = new List<CourseListsByCourseWork>();
                
                var Courses = db.Course.Where(x => x.IsActive == true && x.SignupBeginDate <= DateTime.Now && DateTime.Now <= x.SignupEndDate && x.SubjectNumber.Contains(courseNumber)).OrderBy(x => x.CourseID).ToList();
                foreach (var c in Courses)
                {
                    var eTimePeriod = (c.ClassTime.ClassPeriod + c.NumberOfHours) - 1;
                    var schoolNumber = (c.Classroom == null) ? "不指定教室" : c.Classroom.SchoolNumber;
                    CourseListsByCourseWork courseListsByCourseWork = new CourseListsByCourseWork();
                    courseListsByCourseWork.CourseId = c.CourseID;
                    courseListsByCourseWork.SubjectNumber = c.SubjectNumber;
                    courseListsByCourseWork.Subject = c.Subject;
                    courseListsByCourseWork.Credits = c.Credits;
                    courseListsByCourseWork.TimeLocation = "星期" + c.ClassTime.Week + "(" + c.ClassTime.ClassPeriod + "節~" + eTimePeriod + "節) " + schoolNumber; //ex: 星期一(2節~4節) T20101
                    courseListsByCourseWork.IsCourseLists = false;
                    courseListsByCourseWork.Week = c.ClassTime.Week;
                    courseListsByCourseWork.FTime = c.ClassTime.ClassPeriod;
                    courseListsByCourseWork.ETime = eTimePeriod;

                    courseListsByCourseWorkList.Add(courseListsByCourseWork);
                }

                return Json(new { courseLists = courseListsByCourseWorkList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult CourseSearch(string courseSpecie, string department,int grade, string class1)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<CourseListsByCourseWork> courseListsByCourseWorkList = new List<CourseListsByCourseWork>();

                var courseSorts = (courseSpecie == "系所課程")? CourseSorts.C : CourseSorts.G;
                var Courses = db.Course.Where(x => x.IsActive == true && x.SignupBeginDate <= DateTime.Now && DateTime.Now <= x.SignupEndDate && x.SectionDepartment.CourseSorts == courseSorts && x.SectionDepartment.Department == department && x.Grade == grade && x.Class == class1).OrderBy(x => x.CourseID).ToList();
                foreach (var c in Courses)
                {
                    var eTimePeriod = (c.ClassTime.ClassPeriod + c.NumberOfHours) - 1;
                    var schoolNumber = (c.Classroom == null) ? "不指定教室" : c.Classroom.SchoolNumber;
                    CourseListsByCourseWork courseListsByCourseWork = new CourseListsByCourseWork();
                    courseListsByCourseWork.CourseId = c.CourseID;
                    courseListsByCourseWork.SubjectNumber = c.SubjectNumber;
                    courseListsByCourseWork.Subject = c.Subject;
                    courseListsByCourseWork.Credits = c.Credits;
                    courseListsByCourseWork.TimeLocation = "星期" + c.ClassTime.Week + "(" + c.ClassTime.ClassPeriod + "節~" + eTimePeriod + "節) " + schoolNumber; //ex: 星期一(2節~4節) T20101
                    courseListsByCourseWork.IsCourseLists = false;
                    courseListsByCourseWork.Week = c.ClassTime.Week;
                    courseListsByCourseWork.FTime = c.ClassTime.ClassPeriod;
                    courseListsByCourseWork.ETime = eTimePeriod;

                    courseListsByCourseWorkList.Add(courseListsByCourseWork);
                }

                return Json(new { courseLists = courseListsByCourseWorkList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }


        [HttpPost]
        public JsonResult SaveCourseSelection(List<CourseListsByCartsByCourseWork> deleteByCartOkCopy, List<CourseListsByCartsByCourseWork> courseListsByCarts)
        {
            try
            {
                if (deleteByCartOkCopy == null && courseListsByCarts == null) { return Json(new { message = "選課成功~" }); }

                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var student = db.Student.FirstOrDefault(x => x.LogonId == loginId);

                if (courseListsByCarts != null)
                {
                    string subjectNumberString = "";
                    foreach (var clbc in courseListsByCarts)
                    {
                        var course = db.Course.FirstOrDefault(x => x.CourseID == clbc.CourseId);
                        var courseMax = course.MaxNumber;
                        var electiveCount = db.Elective.Where(x => x.CourseId == clbc.CourseId).Count()+1; //1是指測試增加1個學生會不會超過最大數量

                        if (electiveCount > courseMax)
                        {
                            subjectNumberString += clbc.SubjectNumber +"、";
                        }
                    }

                    if (subjectNumberString != "")
                    {

                        throw new Exception(subjectNumberString.Substring(0, subjectNumberString.Length - 1) + "名額已滿,請把已滿的課程做取消,否則無法繼續下一步!!!");
                    }

                    foreach (var clbc in courseListsByCarts)
                    {
                        var course = db.Course.FirstOrDefault(x => x.CourseID == clbc.CourseId);

                        Elective electiveDto = new Elective();
                        electiveDto.StartingSchoolYear = course.StartingSchoolYear;
                        electiveDto.Semester = course.Semester;
                        electiveDto.Credits = course.Credits;
                        electiveDto.CourseId = clbc.CourseId;
                        electiveDto.LogonId = student.LogonId;
                        electiveDto.StudentId = student.ID;
                        db.Elective.Add(electiveDto);
                        db.SaveChanges();
                        int electiveId = db.Elective.OrderByDescending(x => x.ID).ToList()[0].ID;

                        ElectiveLog electiveLogDto = new ElectiveLog();
                        electiveLogDto.Remarks = "學生自行選課";
                        electiveLogDto.CreatedUserId = student.ID;
                        electiveLogDto.CreateDate = DateTime.Now;
                        electiveLogDto.StartingSchoolYear = course.StartingSchoolYear;
                        electiveLogDto.Semester = course.Semester;
                        electiveLogDto.Credits = course.Credits;
                        electiveLogDto.CourseId = clbc.CourseId;
                        electiveLogDto.LogonId = student.LogonId;
                        electiveLogDto.StudentId = student.ID;
                        electiveLogDto.ElectiveId = electiveId;
                        db.ElectiveLog.Add(electiveLogDto);
                        db.SaveChanges();

                        var electiveCount = db.Elective.Where(x => x.CourseId == clbc.CourseId).Count();
                        var courseStatus = db.CourseStatus.FirstOrDefault(x => x.CourseId == clbc.CourseId);
                        courseStatus.CurrentQuota = electiveCount;
                        db.SaveChanges();

                    }

                }

                if (deleteByCartOkCopy!=null)
                {
                    foreach (var dc in deleteByCartOkCopy)
                    {
                        var elective = db.Elective.FirstOrDefault(x => x.ID == dc.ElectiveId);
                        var CourseId = elective.CourseId;

                        ElectiveLog electiveLogDto = new ElectiveLog();
                        electiveLogDto.Remarks = "學生自行刪除";
                        electiveLogDto.CreatedUserId = student.ID;
                        electiveLogDto.CreateDate = DateTime.Now;
                        electiveLogDto.StartingSchoolYear = elective.StartingSchoolYear;
                        electiveLogDto.Semester = elective.Semester;
                        electiveLogDto.Credits = elective.Credits;
                        electiveLogDto.CourseId = elective.CourseId;
                        electiveLogDto.LogonId = elective.LogonId;
                        electiveLogDto.StudentId = elective.StudentId;
                        electiveLogDto.ElectiveId = elective.ID;
                        db.ElectiveLog.Add(electiveLogDto);
                        db.SaveChanges();

                        db.Elective.Remove(elective);
                        db.SaveChanges();

                        var electiveCount = db.Elective.Where(x => x.CourseId == CourseId).Count();
                        var courseStatus = db.CourseStatus.FirstOrDefault(x => x.CourseId == CourseId);
                        courseStatus.CurrentQuota = electiveCount;
                        db.SaveChanges();

                    }
                }

                return Json(new { message = "選課成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

    }
}