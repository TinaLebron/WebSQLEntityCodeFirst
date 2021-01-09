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
    public class CourseManagementController : Controller
    {
        // GET: CourseManagement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetInfoByAddCourse()
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<string> logonIdList = new List<string>(); 
                List<string> classroomList = new List<string>();
                var employeeCount = db.ApplicationUser.Where(x => x.Identity == Identity.E && x.UserState.ID == 1).Count(); //UserState.ID的1是在職
                var classroomCount = db.Classroom.Where(x => x.IsActive == true).Count();
                if (employeeCount == 0 || classroomCount == 0) { throw new Exception("輸入有誤,如有問題請洽詢(0x)1111111"); }

                var employee = db.ApplicationUser.Where(x => x.Identity == Identity.E && x.UserState.ID == 1).ToList();
                var classroom = db.Classroom.Where(x => x.IsActive == true).ToList();
                foreach (var em in employee)
                {
                    logonIdList.Add(em.LogonId);
                }
                foreach (var c in classroom)
                {
                    classroomList.Add(c.SchoolNumber);
                }

                return Json(new { logonId = logonIdList, classroom = classroomList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult SaveAddInfo(int addSchoolYear, string addSemester, string addSubject, string addSubjectNumber
            , DateTime addCourseDate,DateTime addSignupBeginDate,DateTime addSignupEndDate,int addMinNumber,int addMaxNumber
            , string addCourseSort,string addSection,string addDepartment,int addGrade,string addClass,string addUserName
            , string addRequiredElective,int addCredits,int addNumberOfHours,int addHoursInternship,string addWeek,int addFTime
            ,int addETime,string addSchoolNumber,string addObjectives,string addCourseOutline,string addTextbooks
            ,string addReferenceBooks,string addGrading,string addSchedule)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var semester = (addSemester == "第一學期") ? Semester.F : Semester.S;
                var courseCount = db.Course.Where(x => x.IsActive == true && x.StartingSchoolYear == addSchoolYear && x.Semester == semester && x.SubjectNumber == addSubjectNumber).Count();
                if (courseCount > 0)
                {
                    return Json(new { message = "開課碼 輸入有誤!!!", errorInfo = "addSubjectNumber", error = false });
                }
                var userCount = db.ApplicationUser.Where(x => x.UserStateId == 1 && x.LogonId == addUserName).Count();
                if (userCount == 0)
                {
                    return Json(new { message = "授課教師代碼 輸入有誤!!!", errorInfo = "addUserName", error = false });
                }
                var userId = db.ApplicationUser.FirstOrDefault(x => x.UserStateId == 1 && x.LogonId == addUserName).ID;
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;
                var sectionDepartmentId = db.SectionDepartment.FirstOrDefault(x => x.Section == addSection && x.Department == addDepartment).ID;
                var classTimeId = db.ClassTime.FirstOrDefault(x => x.Week == addWeek && x.ClassPeriod == addFTime).ID;
                var classroomId = 0;
                if (addSchoolNumber != "")
                {
                    var classroomCount = db.Classroom.Where(x => x.SchoolNumber == addSchoolNumber).Count();
                    if (classroomCount == 0)
                    {
                        return Json(new { message = "上課地點 輸入有誤!!!", errorInfo = "addSchoolNumber", error = false });
                    }
                    classroomId = db.Classroom.FirstOrDefault(x => x.SchoolNumber == addSchoolNumber).ID;
                }
                
                //檢查教室時段是否有衝突
                var course1Count = db.Course.Where(x => x.IsActive == true && x.StartingSchoolYear == addSchoolYear && x.Semester == semester && x.ClassroomId == classroomId && x.ClassTime.Week == addWeek).Count();
                if (course1Count != 0)
                {
                    var course = db.Course.Where(x => x.IsActive == true && x.StartingSchoolYear == addSchoolYear && x.Semester == semester && x.ClassroomId == classroomId && x.ClassTime.Week == addWeek).ToList();
                    for (int i = addFTime; i <= addETime; i++)
                    {
                        for (int j = 0; j < course1Count; j++)
                        {
                            int cp = course[j].ClassTime.ClassPeriod;
                            int noh = (cp + course[j].NumberOfHours)-1;
                            if (cp <= i && i <= noh)
                            {
                                return Json(new { message = "已經有其他課程選擇此教室的時段,請在作確認!!!", errorInfo = "addFTime", error = false });
                            }
                            
                        }
                        
                    }

                }

                CourseDescription courseDescription = new CourseDescription();
                courseDescription.Objectives = addObjectives.Replace("\n", "<br />");
                courseDescription.CourseOutline = addCourseOutline.Replace("\n", "<br />");
                courseDescription.Textbooks = addTextbooks.Replace("\n", "<br />");
                courseDescription.ReferenceBooks = addReferenceBooks.Replace("\n", "<br />");
                courseDescription.Grading = addGrading.Replace("\n", "<br />");
                courseDescription.Schedule = addSchedule.Replace("\n", "<br />");
                db.CourseDescription.Add(courseDescription);
                db.SaveChanges();
                int courseDescriptionId = db.CourseDescription.OrderByDescending(x=>x.ID).ToList()[0].ID;


                Course courseDto = new Course();
                courseDto.Subject = addSubject;
                courseDto.SubjectNumber = addSubjectNumber;
                courseDto.Credits = addCredits;
                courseDto.StartingSchoolYear = addSchoolYear;
                courseDto.Semester = (addSemester== "第一學期")? Semester.F:Semester.S;
                courseDto.Grade = addGrade;
                courseDto.Class = addClass;
                courseDto.RequiredElective = (addRequiredElective== "必修")? RequiredElective.R : RequiredElective.E;
                courseDto.NumberOfHours = addNumberOfHours;
                courseDto.HoursInternship = addHoursInternship;
                courseDto.CourseDate = DateTime.Parse(addCourseDate.Year + "-" + addCourseDate.Month + "-" + addCourseDate.Day);
                courseDto.SignupBeginDate = DateTime.Parse(addSignupBeginDate.Year + "-" + addSignupBeginDate.Month + "-" + addSignupBeginDate.Day); 
                courseDto.SignupEndDate = DateTime.Parse(addSignupEndDate.Year + "-" + addSignupEndDate.Month + "-" + addSignupEndDate.Day); 
                courseDto.MinNumber = addMinNumber;
                courseDto.MaxNumber = addMaxNumber;
                courseDto.LogonId = addUserName;
                courseDto.Title = "新增課程資訊";
                courseDto.CreatedUserId = createdUserId;
                courseDto.CreateDate = DateTime.Now;
                courseDto.IsActive = true;
                courseDto.CourseDescriptionId = courseDescriptionId;
                courseDto.SectionDepartmentId = sectionDepartmentId;
                courseDto.ClassTimeId = classTimeId;
                if (addSchoolNumber != "")
                {
                    courseDto.ClassroomId = classroomId;
                }
                courseDto.ApplicationUserId = userId;
                db.Course.Add(courseDto);
                db.SaveChanges();
                int courseDtoId = db.Course.OrderByDescending(x => x.CourseID).ToList()[0].CourseID;

                CourseLog CourseLogDto = new CourseLog();
                CourseLogDto.Remarks = "新增課程資訊";
                CourseLogDto.Subject = addSubject;
                CourseLogDto.SubjectNumber = addSubjectNumber;
                CourseLogDto.Credits = addCredits;
                CourseLogDto.StartingSchoolYear = addSchoolYear;
                CourseLogDto.Semester = (addSemester == "第一學期") ? Semester.F : Semester.S;
                CourseLogDto.Grade = addGrade;
                CourseLogDto.Class = addClass;
                CourseLogDto.RequiredElective = (addRequiredElective == "必修") ? RequiredElective.R : RequiredElective.E;
                CourseLogDto.NumberOfHours = addNumberOfHours;
                CourseLogDto.HoursInternship = addHoursInternship;
                CourseLogDto.CourseDate = DateTime.Parse(addCourseDate.Year + "-" + addCourseDate.Month + "-" + addCourseDate.Day);
                CourseLogDto.SignupBeginDate = DateTime.Parse(addSignupBeginDate.Year + "-" + addSignupBeginDate.Month + "-" + addSignupBeginDate.Day);
                CourseLogDto.SignupEndDate = DateTime.Parse(addSignupEndDate.Year + "-" + addSignupEndDate.Month + "-" + addSignupEndDate.Day);
                CourseLogDto.MinNumber = addMinNumber;
                CourseLogDto.MaxNumber = addMaxNumber;
                CourseLogDto.LogonId = addUserName;
                CourseLogDto.Title = courseDto.Title;
                CourseLogDto.CreatedUserId = createdUserId;
                CourseLogDto.CreateDate = DateTime.Now;
                CourseLogDto.CourseDescriptionId = courseDescriptionId;
                CourseLogDto.SectionDepartmentId = sectionDepartmentId;
                CourseLogDto.ClassTimeId = classTimeId;
                if (addSchoolNumber != "")
                {
                    CourseLogDto.ClassroomId = classroomId;
                }
                CourseLogDto.ApplicationUserId = userId;
                CourseLogDto.CourseId = courseDtoId;
                db.CourseLog.Add(CourseLogDto);
                db.SaveChanges();

                ChooseAClassroom chooseAClassroomDto = new ChooseAClassroom();
                chooseAClassroomDto.StartingSchoolYear = addSchoolYear;
                chooseAClassroomDto.Semester = (addSemester == "第一學期") ? Semester.F : Semester.S;
                chooseAClassroomDto.NumberOfHours = addNumberOfHours;
                chooseAClassroomDto.ClassTimeId = classTimeId;
                if (addSchoolNumber != "")
                {
                    chooseAClassroomDto.ClassroomId = classroomId;
                }
                chooseAClassroomDto.CourseId = courseDtoId;
                chooseAClassroomDto.LogonId = addUserName;
                chooseAClassroomDto.ApplicationUserId = userId;
                chooseAClassroomDto.Remarks = "選課作業";
                db.ChooseAClassroom.Add(chooseAClassroomDto);
                db.SaveChanges();
                int chooseAClassroomId = db.ChooseAClassroom.OrderByDescending(x => x.ID).ToList()[0].ID;

                ChooseAClassroomLog chooseAClassroomLogDto = new ChooseAClassroomLog();
                chooseAClassroomLogDto.Remarks = "選課作業";
                chooseAClassroomLogDto.CreatedUserId = createdUserId;
                chooseAClassroomLogDto.CreateDate = DateTime.Now;
                chooseAClassroomLogDto.NumberOfHours = addNumberOfHours;
                chooseAClassroomLogDto.ClassTimeId = classTimeId;
                if (addSchoolNumber != "")
                {
                    chooseAClassroomLogDto.ClassroomId = classroomId;
                }
                chooseAClassroomLogDto.CourseId = courseDtoId;
                chooseAClassroomLogDto.LogonId = addUserName;
                chooseAClassroomLogDto.ApplicationUserId = userId;
                chooseAClassroomLogDto.ChooseAClassroomId = chooseAClassroomId;
                db.ChooseAClassroomLog.Add(chooseAClassroomLogDto);
                db.SaveChanges();

                

                return Json(new { message = "新增成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, errorInfo = "", error = false });
            }
        }
        
        [HttpGet]
        public ActionResult EditCourse(int courseID)
        {
            SchoolContext db = new SchoolContext();
            List<string> logonIdList = new List<string>();
            List<string> classroomList = new List<string>();
            List<string> sectionList = new List<string>();
            List<string> departmentList = new List<string>();
            var courses = db.Course.FirstOrDefault(x => x.CourseID == courseID);
            var SectionDepartmentId = courses.SectionDepartmentId;
            var sectionDepartments = db.SectionDepartment.FirstOrDefault(x => x.ID == SectionDepartmentId);

            var courseDate = courses.CourseDate.Date;
            var classTimes = db.ClassTime.FirstOrDefault(x => x.ID == courses.ClassTimeId);
            var week = classTimes.Week;
            var fTime = classTimes.ClassPeriod;
            var eTime = (classTimes.ClassPeriod + courses.NumberOfHours) - 1;
            var schoolNumber = "";

            if (courses.ClassroomId != null)
            {
                schoolNumber = db.Classroom.FirstOrDefault(x => x.ID == courses.ClassroomId).SchoolNumber;
            }
            

            var employee = db.ApplicationUser.Where(x => x.Identity == Identity.E && x.UserState.ID == 1).ToList();
            var classroom = db.Classroom.Where(x => x.IsActive == true).ToList();


            foreach (var em in employee)
            {
                logonIdList.Add(em.LogonId);
            }
            foreach (var c in classroom)
            {
                classroomList.Add(c.SchoolNumber);
            }

            if (courses.SectionDepartment.CourseSorts == CourseSorts.C)
            {
                var sectionDepartment = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == courses.SectionDepartment.Section).ToList();
                var section = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C).Select(c => new { Section = c.Section }).Distinct().ToList();
                foreach (var sd in sectionDepartment)
                {
                    departmentList.Add(sd.Department);
                }
                foreach (var s in section)
                {
                    sectionList.Add(s.Section);
                }


            }
            else if (courses.SectionDepartment.CourseSorts == CourseSorts.G)
            {
                var sectionDepartment = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G && x.Section == courses.SectionDepartment.Section).ToList();
                var section = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.G).Select(c => new { Section = c.Section }).Distinct().ToList();
                foreach (var sd in sectionDepartment)
                {
                    departmentList.Add(sd.Department);
                }
                foreach (var s in section)
                {
                    sectionList.Add(s.Section);
                }
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
            if (sectionDepartments.CourseSorts == CourseSorts.C) {
                ViewBag.editSections1 = sectionList;
                ViewBag.editSection1 = sectionDepartments.Section;
                ViewBag.editDepartments1 = departmentList;
                ViewBag.editDepartment1 = sectionDepartments.Department;
                ViewBag.editGrade1 = courses.Grade;
                ViewBag.editClass1 = courses.Class;

                ViewBag.editSections2 = "[]";
                ViewBag.editSection2 = "";
                ViewBag.editDepartments2 = "[]";
                ViewBag.editDepartment2 = "";
                ViewBag.editGrade2 = 0;
            }
            else if (sectionDepartments.CourseSorts == CourseSorts.G) {
                ViewBag.editSections2 = sectionList;
                ViewBag.editSection2 = sectionDepartments.Section;
                ViewBag.editDepartments2 = departmentList;
                ViewBag.editDepartment2 = sectionDepartments.Department;
                ViewBag.editGrade2 = courses.Grade;

                ViewBag.editSections1 = "[]";
                ViewBag.editSection1 = "";
                ViewBag.editDepartments1 = "[]";
                ViewBag.editDepartment1 = "";
                ViewBag.editGrade1 = 0;
                ViewBag.editClass1 = "";
            }
            ViewBag.editUserNames = logonIdList;
            ViewBag.editUserName = courses.ApplicationUser.LogonId;
            ViewBag.editRequiredElective = (courses.RequiredElective == 0) ? "必修" : "選修";
            ViewBag.editCredits = courses.Credits;
            ViewBag.editNumberOfHours = courses.NumberOfHours;
            ViewBag.editHoursInternship = courses.HoursInternship;
            ViewBag.editWeek = week;
            ViewBag.editFTime = fTime;
            ViewBag.editETime = eTime;
            ViewBag.editSchoolNumbers = classroomList;
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
        public JsonResult SaveEditInfo(int editSchoolYear, string editSemester, string editSubject, string editSubjectNumber
            , DateTime editCourseDate, DateTime editSignupBeginDate, DateTime editSignupEndDate, int editMinNumber, int editMaxNumber
            , string editCourseSort, string editSection, string editDepartment, int editGrade, string editClass, string editUserName
            , string editRequiredElective, int editCredits, int editNumberOfHours, int editHoursInternship, string editWeek, int editFTime
            , int editETime, string editSchoolNumber, string editObjectives, string editCourseOutline, string editTextbooks
            , string editReferenceBooks, string editGrading, string editSchedule,int courseID)
        {
            try
            {
                SchoolContext db = new SchoolContext();

                string loginId = Session["sIDNo"].ToString();
                var semester = (editSemester == "第一學期") ? Semester.F: Semester.S;
                var courseCount = db.Course.Where(x => x.IsActive == true && x.CourseID != courseID && x.StartingSchoolYear == editSchoolYear && x.Semester == semester && x.SubjectNumber == editSubjectNumber).Count();
                if (courseCount > 0)
                {
                    return Json(new { message = "開課碼 輸入有誤!!!", errorInfo = "editSubjectNumber", error = false });
                }

                var electiveCount = db.Elective.Where(x => x.CourseId == courseID).Count();
                if (electiveCount > editMaxNumber)
                {
                    return Json(new { message = "有"+ electiveCount + "位學生已選課,則最高人數不得低於"+ electiveCount + "!!!", errorInfo = "editMaxNumber", error = false });
                }

                var userCount = db.ApplicationUser.Where(x => x.UserStateId == 1 && x.LogonId == editUserName).Count();
                if (userCount == 0)
                {
                    return Json(new { message = "授課教師代碼 輸入有誤!!!", errorInfo = "editUserName", error = false });
                }
                var userId = db.ApplicationUser.FirstOrDefault(x => x.UserStateId == 1 && x.LogonId == editUserName).ID;
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;
                var sectionDepartmentId = db.SectionDepartment.FirstOrDefault(x => x.Section == editSection && x.Department == editDepartment).ID;
                var classTimeId = db.ClassTime.FirstOrDefault(x => x.Week == editWeek && x.ClassPeriod == editFTime).ID;
                var classroomId = 0;
                if (editSchoolNumber != "")
                {
                    var classroomCount = db.Classroom.Where(x => x.SchoolNumber == editSchoolNumber).Count();
                    if (classroomCount == 0)
                    {
                        return Json(new { message = "上課地點 輸入有誤!!!", errorInfo = "editSchoolNumber", error = false });
                    }
                    classroomId = db.Classroom.FirstOrDefault(x => x.SchoolNumber == editSchoolNumber).ID;
                }
                
                //檢查教室時段是否有衝突
                var course1Count = db.Course.Where(x => x.IsActive == true && x.CourseID != courseID && x.StartingSchoolYear == editSchoolYear && x.Semester == semester && x.ClassroomId == classroomId && x.ClassTime.Week == editWeek).Count();
                if (course1Count != 0)
                {
                    var course = db.Course.Where(x => x.IsActive == true && x.CourseID != courseID && x.StartingSchoolYear == editSchoolYear && x.Semester == semester && x.ClassroomId == classroomId && x.ClassTime.Week == editWeek).ToList();
                    for (int i = editFTime; i <= editETime; i++)
                    {
                        for (int j = 0; j < course1Count; j++)
                        {
                            int cp = course[j].ClassTime.ClassPeriod;
                            int noh = (cp + course[j].NumberOfHours) - 1;
                            if (cp <= i && i <= noh)
                            {
                                return Json(new { message = "已經有其他課程選擇此教室的時段,請在作確認!!!", errorInfo = "editFTime", error = false });
                            }

                        }

                    }

                }

                var courseByEdit = db.Course.FirstOrDefault(x=>x.CourseID == courseID);
                var courseDescriptionByEdit = db.CourseDescription.FirstOrDefault(x => x.ID == courseByEdit.CourseDescriptionId);
                courseDescriptionByEdit.Objectives = editObjectives.Replace("\n", "<br />");
                courseDescriptionByEdit.CourseOutline = editCourseOutline.Replace("\n", "<br />");
                courseDescriptionByEdit.Textbooks = editTextbooks.Replace("\n", "<br />");
                courseDescriptionByEdit.ReferenceBooks = editReferenceBooks.Replace("\n", "<br />");
                courseDescriptionByEdit.Grading = editGrading.Replace("\n", "<br />");
                courseDescriptionByEdit.Schedule = editSchedule.Replace("\n", "<br />");
                db.SaveChanges();

                courseByEdit.Subject = editSubject;
                courseByEdit.SubjectNumber = editSubjectNumber;
                courseByEdit.Credits = editCredits;
                courseByEdit.StartingSchoolYear = editSchoolYear;
                courseByEdit.Semester = (editSemester == "第一學期") ? Semester.F : Semester.S;
                courseByEdit.Grade = editGrade;
                courseByEdit.Class = editClass;
                courseByEdit.RequiredElective = (editRequiredElective == "必修") ? RequiredElective.R : RequiredElective.E;
                courseByEdit.NumberOfHours = editNumberOfHours;
                courseByEdit.HoursInternship = editHoursInternship;
                courseByEdit.CourseDate = DateTime.Parse(editCourseDate.Year + "-" + editCourseDate.Month + "-" + editCourseDate.Day);
                courseByEdit.SignupBeginDate = DateTime.Parse(editSignupBeginDate.Year + "-" + editSignupBeginDate.Month + "-" + editSignupBeginDate.Day);
                courseByEdit.SignupEndDate = DateTime.Parse(editSignupEndDate.Year + "-" + editSignupEndDate.Month + "-" + editSignupEndDate.Day);
                courseByEdit.MinNumber = editMinNumber;
                courseByEdit.MaxNumber = editMaxNumber;
                courseByEdit.LogonId = editUserName;
                courseByEdit.Title = "編輯課程資訊";
                courseByEdit.LastModifiedUserId = createdUserId;
                courseByEdit.LastModifyDate = DateTime.Now;
                courseByEdit.IsActive = true;
                courseByEdit.CourseDescriptionId = courseDescriptionByEdit.ID;
                courseByEdit.SectionDepartmentId = sectionDepartmentId;
                courseByEdit.ClassTimeId = classTimeId;
                if (editSchoolNumber != "")
                {
                    courseByEdit.ClassroomId = classroomId;
                }
                courseByEdit.ApplicationUserId = userId;
                db.SaveChanges();


                CourseLog CourseLogDto = new CourseLog();
                CourseLogDto.Remarks = "編輯課程資訊";
                CourseLogDto.Subject = editSubject;
                CourseLogDto.SubjectNumber = editSubjectNumber;
                CourseLogDto.Credits = editCredits;
                CourseLogDto.StartingSchoolYear = editSchoolYear;
                CourseLogDto.Semester = (editSemester == "第一學期") ? Semester.F : Semester.S;
                CourseLogDto.Grade = editGrade;
                CourseLogDto.Class = editClass;
                CourseLogDto.RequiredElective = (editRequiredElective == "必修") ? RequiredElective.R : RequiredElective.E;
                CourseLogDto.NumberOfHours = editNumberOfHours;
                CourseLogDto.HoursInternship = editHoursInternship;
                CourseLogDto.CourseDate = DateTime.Parse(editCourseDate.Year + "-" + editCourseDate.Month + "-" + editCourseDate.Day);
                CourseLogDto.SignupBeginDate = DateTime.Parse(editSignupBeginDate.Year + "-" + editSignupBeginDate.Month + "-" + editSignupBeginDate.Day);
                CourseLogDto.SignupEndDate = DateTime.Parse(editSignupEndDate.Year + "-" + editSignupEndDate.Month + "-" + editSignupEndDate.Day);
                CourseLogDto.MinNumber = editMinNumber;
                CourseLogDto.MaxNumber = editMaxNumber;
                CourseLogDto.LogonId = editUserName;
                CourseLogDto.Title = courseByEdit.Title;
                CourseLogDto.CreatedUserId = createdUserId;
                CourseLogDto.CreateDate = DateTime.Now;
                CourseLogDto.CourseDescriptionId = courseDescriptionByEdit.ID;
                CourseLogDto.SectionDepartmentId = sectionDepartmentId;
                CourseLogDto.ClassTimeId = classTimeId;
                if (editSchoolNumber != "")
                {
                    CourseLogDto.ClassroomId = classroomId;
                }
                CourseLogDto.ApplicationUserId = userId;
                CourseLogDto.CourseId = courseID;
                db.CourseLog.Add(CourseLogDto);
                db.SaveChanges();


                var chooseAClassroomByEdit = db.ChooseAClassroom.FirstOrDefault(x => x.CourseId == courseID);
                chooseAClassroomByEdit.StartingSchoolYear = editSchoolYear;
                chooseAClassroomByEdit.Semester = (editSemester == "第一學期") ? Semester.F : Semester.S;
                chooseAClassroomByEdit.NumberOfHours = editNumberOfHours;
                chooseAClassroomByEdit.ClassTimeId = classTimeId;
                if (editSchoolNumber != "")
                {
                    chooseAClassroomByEdit.ClassroomId = classroomId;
                }
                chooseAClassroomByEdit.LogonId = editUserName;
                chooseAClassroomByEdit.ApplicationUserId = userId;
                chooseAClassroomByEdit.Remarks = "編輯選課作業";
                db.SaveChanges();


                ChooseAClassroomLog chooseAClassroomLogDto = new ChooseAClassroomLog();
                chooseAClassroomLogDto.Remarks = "編輯選課作業";
                chooseAClassroomLogDto.CreatedUserId = createdUserId;
                chooseAClassroomLogDto.CreateDate = DateTime.Now;
                chooseAClassroomLogDto.NumberOfHours = editNumberOfHours;
                chooseAClassroomLogDto.ClassTimeId = classTimeId;
                if (editSchoolNumber != "")
                {
                    chooseAClassroomLogDto.ClassroomId = classroomId;
                }
                chooseAClassroomLogDto.CourseId = courseID;
                chooseAClassroomLogDto.LogonId = editUserName;
                chooseAClassroomLogDto.ApplicationUserId = userId;
                chooseAClassroomLogDto.ChooseAClassroomId = chooseAClassroomByEdit.ID;
                db.ChooseAClassroomLog.Add(chooseAClassroomLogDto);
                db.SaveChanges();

                return Json(new { message = "編輯成功~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, errorInfo = "", error = false });
            }
        }

        [HttpGet]
        public ActionResult DeleteCourse(int courseID)
        {
            SchoolContext db = new SchoolContext();
            string content = "";
            var courseDeleteCount = db.Course.FirstOrDefault(x => x.CourseID == courseID);
            var electiveLogDeleteCount = db.Elective.Where(x => x.CourseId == courseID).Count();
            if (electiveLogDeleteCount == 0)
            {
                content = "確定要刪除科目代碼為" + courseDeleteCount.SubjectNumber + "?";
            }
            else
            {
                content = "確定要刪除科目代碼為" + courseDeleteCount.SubjectNumber + "?<br/>" + courseDeleteCount.SubjectNumber + "已有學生報名,若要刪除則系統會自動幫學生做取消";
            }


            ViewBag.courseID = courseID;
            ViewBag.content = content;

            return View();
        }

        [HttpPost]
        public JsonResult SaveDeleteCourse(int courseID, string remarks)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;

                var courseDelete = db.Course.FirstOrDefault(x => x.CourseID == courseID);
                courseDelete.IsActive = false;
                db.SaveChanges();

                CourseLog CourseLogDto = new CourseLog();
                CourseLogDto.Remarks = remarks + "-取消選課作業(管理員)";
                CourseLogDto.Subject = courseDelete.Subject;
                CourseLogDto.SubjectNumber = courseDelete.SubjectNumber;
                CourseLogDto.Credits = courseDelete.Credits;
                CourseLogDto.StartingSchoolYear = courseDelete.StartingSchoolYear;
                CourseLogDto.Semester = courseDelete.Semester;
                CourseLogDto.Grade = courseDelete.Grade;
                CourseLogDto.Class = courseDelete.Class;
                CourseLogDto.RequiredElective = courseDelete.RequiredElective;
                CourseLogDto.NumberOfHours = courseDelete.NumberOfHours;
                CourseLogDto.HoursInternship = courseDelete.HoursInternship;
                CourseLogDto.CourseDate = courseDelete.CourseDate;
                CourseLogDto.SignupBeginDate = courseDelete.SignupBeginDate;
                CourseLogDto.SignupEndDate = courseDelete.SignupEndDate;
                CourseLogDto.MinNumber = courseDelete.MinNumber;
                CourseLogDto.MaxNumber = courseDelete.MaxNumber;
                CourseLogDto.LogonId = courseDelete.LogonId;
                CourseLogDto.Title = courseDelete.Title;
                CourseLogDto.CreatedUserId = createdUserId;
                CourseLogDto.CreateDate = DateTime.Now;
                CourseLogDto.CourseDescriptionId = courseDelete.CourseDescriptionId;
                CourseLogDto.SectionDepartmentId = courseDelete.SectionDepartmentId;
                CourseLogDto.ClassTimeId = courseDelete.ClassTimeId;
                CourseLogDto.ClassroomId = courseDelete.ClassroomId;
                CourseLogDto.ApplicationUserId = courseDelete.ApplicationUserId;
                CourseLogDto.CourseId = courseID;
                db.CourseLog.Add(CourseLogDto);
                db.SaveChanges();

                var chooseAClassroomDelete = db.ChooseAClassroom.FirstOrDefault(x => x.CourseId == courseID); //只會有一筆資料
                ChooseAClassroomLog chooseAClassroomLogDto = new ChooseAClassroomLog();
                chooseAClassroomLogDto.Remarks = remarks + "-取消選課作業(管理員)";
                chooseAClassroomLogDto.CreatedUserId = createdUserId;
                chooseAClassroomLogDto.CreateDate = DateTime.Now;
                chooseAClassroomLogDto.NumberOfHours = chooseAClassroomDelete.NumberOfHours;
                chooseAClassroomLogDto.ClassTimeId = chooseAClassroomDelete.ClassTimeId;
                chooseAClassroomLogDto.ClassroomId = chooseAClassroomDelete.ClassroomId;
                chooseAClassroomLogDto.CourseId = courseID;
                chooseAClassroomLogDto.LogonId = chooseAClassroomDelete.LogonId;
                chooseAClassroomLogDto.ApplicationUserId = chooseAClassroomDelete.ApplicationUserId;
                chooseAClassroomLogDto.ChooseAClassroomId = chooseAClassroomDelete.ID;
                db.ChooseAClassroomLog.Add(chooseAClassroomLogDto);
                db.SaveChanges();

                db.ChooseAClassroom.Remove(chooseAClassroomDelete);
                db.SaveChanges();

                var electiveDelete = db.Elective.Where(x => x.CourseId == courseID).ToList();
                var count = electiveDelete.Count();
                if (count == 0)
                {
                    return Json(new { message = "刪除成功~" });
                }

                foreach (var e in electiveDelete)
                {
                    ElectiveLog electiveLogDto = new ElectiveLog();
                    electiveLogDto.Remarks = remarks + "-取消選課作業(管理員)";
                    electiveLogDto.CreatedUserId = createdUserId;
                    electiveLogDto.CreateDate = DateTime.Now;
                    electiveLogDto.StartingSchoolYear = e.StartingSchoolYear;
                    electiveLogDto.Semester = e.Semester;
                    electiveLogDto.Credits = e.Credits;
                    electiveLogDto.CourseId = e.CourseId;
                    electiveLogDto.LogonId = e.LogonId;
                    electiveLogDto.StudentId = e.StudentId;
                    electiveLogDto.ElectiveId = e.ID;
                    db.ElectiveLog.Add(electiveLogDto);
                    db.SaveChanges();

                    db.Elective.Remove(e);
                    db.SaveChanges();

                }

                return Json(new { message = "此課程已成功取消" + count + "位學生~" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

        [HttpGet]
        public ActionResult ElectiveCourse(int courseID)
        {
            ViewBag.courseID = courseID;
            return View();
        }

        [HttpPost]
        public JsonResult GetElectiveListInitial(int courseID)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<ElectiveCourseListDto> electiveCourseListDtoList = new List<ElectiveCourseListDto>();

                var electives = db.Elective.Where(x => x.CourseId == courseID).ToList();
                for (int i = 0; i < electives.Count(); i++)
                {
                    var electivesStudentId = electives[i].StudentId;
                    var student = db.Student.FirstOrDefault(x => x.ID == electivesStudentId);
                    ElectiveCourseListDto electiveCourseListDto = new ElectiveCourseListDto
                    {
                        ElectivesID = electives[i].ID,
                        CourseID = electives[i].CourseId,
                        SubjectNumber = electives[i].Course.SubjectNumber,
                        StudentId = electives[i].StudentId,
                        UserClass = student.SectionDepartment.Section+" " 
                        + student.SectionDepartment.DepartmentAbbreviation 
                        + student.Grade 
                        + student.Class, //ex: 大學部 資工系1A
                        StudentLogonId = electives[i].LogonId,
                        Number = i + 1

                    };
                    electiveCourseListDtoList.Add(electiveCourseListDto);

                    
                }


                return Json(new { electiveCourseList = electiveCourseListDtoList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

        [HttpPost]
        public JsonResult ElectiveListSearch(int courseID, string logonId, List<ElectiveCourseListDto> electiveCourseList)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<ElectiveCourseListDto> electiveCourseListDtoList = new List<ElectiveCourseListDto>();

                var electives = db.Elective.Where(x => x.CourseId == courseID && x.LogonId.Contains(logonId)).ToList();
                for (int i = 0; i < electives.Count(); i++)
                {
                    var electivesStudentId = electives[i].StudentId;
                    var student = db.Student.FirstOrDefault(x => x.ID == electivesStudentId);
                    int number = 1;

                    foreach (var ecl in electiveCourseList)
                    {
                        if (ecl.ElectivesID == electives[i].ID)
                        {
                            number = ecl.Number;
                            break;
                        }
                    }
                    
                    ElectiveCourseListDto electiveCourseListDto = new ElectiveCourseListDto
                    {
                        ElectivesID = electives[i].ID,
                        CourseID = electives[i].CourseId,
                        SubjectNumber = electives[i].Course.SubjectNumber,
                        StudentId = electives[i].StudentId,
                        UserClass = student.SectionDepartment.Section + " "
                        + student.SectionDepartment.DepartmentAbbreviation
                        + student.Grade
                        + student.Class, //ex: 大學部 資工系1A
                        StudentLogonId = electives[i].LogonId,
                        Number = number

                    };
                    electiveCourseListDtoList.Add(electiveCourseListDto);


                }


                return Json(new { electiveCourseList = electiveCourseListDtoList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

        [HttpGet]
        public ActionResult DeleteElectiveCourseList(int electivesID, string studentLogonId,string subjectNumber,int courseID)
        {
            ViewBag.content = "您確定要幫"+ studentLogonId + "取消"+ subjectNumber + "的課程?";
            ViewBag.electivesID = electivesID;
            ViewBag.studentLogonId = studentLogonId;
            ViewBag.courseID = courseID;

            return View();
        }

        [HttpPost]
        public JsonResult SaveDeleteElectiveList(int electivesID, string StudentLogonId, string remarks)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;

                

                var electiveDelete = db.Elective.FirstOrDefault(x => x.ID == electivesID );

                ElectiveLog electiveLogDto = new ElectiveLog();
                electiveLogDto.Remarks = remarks + "-取消選課作業(管理員)";
                electiveLogDto.CreatedUserId = createdUserId;
                electiveLogDto.CreateDate = DateTime.Now;
                electiveLogDto.StartingSchoolYear = electiveDelete.StartingSchoolYear;
                electiveLogDto.Semester = electiveDelete.Semester;
                electiveLogDto.Credits = electiveDelete.Credits;
                electiveLogDto.CourseId = electiveDelete.CourseId;
                electiveLogDto.LogonId = electiveDelete.LogonId;
                electiveLogDto.StudentId = electiveDelete.StudentId;
                electiveLogDto.ElectiveId = electivesID;
                db.ElectiveLog.Add(electiveLogDto);
                db.SaveChanges();

                db.Elective.Remove(electiveDelete);
                db.SaveChanges();

                return Json(new { message = StudentLogonId + "已成功取消選課" });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

        [HttpPost]
        public JsonResult GetStudentsListToElectiveCourse(int courseID)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<string> departmentList = new List<string>();
                departmentList.Add("All");
                List<GetStudentsListToElectiveCourse> getStudentsListToElectiveCourseList = new List<GetStudentsListToElectiveCourse>();
                var course = db.Course.FirstOrDefault(x => x.CourseID == courseID);
                var section = course.SectionDepartment.Section;
                var sectionDepartments = db.SectionDepartment.Where(x => x.CourseSorts == CourseSorts.C && x.Section == section).ToList();
               

                foreach (var sd in sectionDepartments)
                {
                    var students = db.Student.Where(x => x.SectionDepartmentId == sd.ID && x.IsActive == true && (x.UserStateId == 4 || x.UserStateId == 8)).OrderBy(x=>x.Grade).ToList();
                    List<StudentsInfo> studentsInfoList = new List<StudentsInfo>();
                    foreach (var s in students)
                    {
                        var electiveCount = db.Elective.Where(x=>x.CourseId == courseID && x.StudentId == s.ID).Count();
                        if (electiveCount == 0)
                        {
                            StudentsInfo studentsInfo = new StudentsInfo();
                            studentsInfo.LogonId = s.LogonId;
                            studentsInfo.ClassLogonId = s.Grade + s.Class + "-" + s.LogonId; // ex: 1A A115940002
                            studentsInfoList.Add(studentsInfo);
                        }
                        
                    }

                    var getStudentsListToElectiveCourse = new GetStudentsListToElectiveCourse();
                    getStudentsListToElectiveCourse.DepartmentAbbreviation = sd.DepartmentAbbreviation;
                    getStudentsListToElectiveCourse.StudentsInfos = studentsInfoList;

                    getStudentsListToElectiveCourseList.Add(getStudentsListToElectiveCourse);
                    departmentList.Add(sd.DepartmentAbbreviation);

                }


                return Json(new { departments = departmentList, studentsLists = getStudentsListToElectiveCourseList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

        [HttpPost]
        public JsonResult SaveAddElectiveCourseList(int courseID, List<string> studentsLogonIdOption)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                string loginId = Session["sIDNo"].ToString();
                var createdUserId = db.ApplicationUser.FirstOrDefault(x => x.LogonId == loginId).ID;
                var courseMaxNumber = db.Course.FirstOrDefault(x => x.CourseID == courseID).MaxNumber;
                var courseSubjectNumber = db.Course.FirstOrDefault(x => x.CourseID == courseID).SubjectNumber;
                string Message = studentsLogonIdOption.Count() + "位學生選課成功~"; ;


                foreach (var sl in studentsLogonIdOption)
                {
                    var student = db.Student.FirstOrDefault(x => x.LogonId == sl);
                    var course = db.Course.FirstOrDefault(x => x.CourseID == courseID);

                    Elective electiveDto = new Elective();
                    electiveDto.StartingSchoolYear = course.StartingSchoolYear;
                    electiveDto.Semester = course.Semester;
                    electiveDto.Credits = course.Credits;
                    electiveDto.CourseId = courseID;
                    electiveDto.LogonId = student.LogonId;
                    electiveDto.StudentId = student.ID;
                    db.Elective.Add(electiveDto);
                    db.SaveChanges();
                    int electiveId = db.Elective.OrderByDescending(x => x.ID).ToList()[0].ID;

                    ElectiveLog electiveLogDto = new ElectiveLog();
                    electiveLogDto.Remarks = "幫學生選課程(管理員)";
                    electiveLogDto.CreatedUserId = createdUserId;
                    electiveLogDto.CreateDate = DateTime.Now;
                    electiveLogDto.StartingSchoolYear = course.StartingSchoolYear;
                    electiveLogDto.Semester = course.Semester;
                    electiveLogDto.Credits = course.Credits;
                    electiveLogDto.CourseId = courseID;
                    electiveLogDto.LogonId = student.LogonId;
                    electiveLogDto.StudentId = student.ID;
                    electiveLogDto.ElectiveId = electiveId;
                    db.ElectiveLog.Add(electiveLogDto);
                    db.SaveChanges();

                }

                var currentQuotaCount = db.Elective.Where(x => x.CourseId == courseID).Count();
                if (currentQuotaCount > courseMaxNumber)
                {
                    var course = db.Course.FirstOrDefault(x => x.CourseID == courseID);
                    course.MaxNumber = currentQuotaCount;
                    db.SaveChanges();

                    CourseLog CourseLogDto = new CourseLog();
                    CourseLogDto.Remarks = "已超過上限人數,系統自動增加上限人數(管理員)";
                    CourseLogDto.Subject = course.Subject;
                    CourseLogDto.SubjectNumber = course.SubjectNumber;
                    CourseLogDto.Credits = course.Credits;
                    CourseLogDto.StartingSchoolYear = course.StartingSchoolYear;
                    CourseLogDto.Semester = course.Semester;
                    CourseLogDto.Grade = course.Grade;
                    CourseLogDto.Class = course.Class;
                    CourseLogDto.RequiredElective = course.RequiredElective;
                    CourseLogDto.NumberOfHours = course.NumberOfHours;
                    CourseLogDto.HoursInternship = course.HoursInternship;
                    CourseLogDto.CourseDate = course.CourseDate;
                    CourseLogDto.SignupBeginDate = course.SignupBeginDate;
                    CourseLogDto.SignupEndDate = course.SignupEndDate;
                    CourseLogDto.MinNumber = course.MinNumber;
                    CourseLogDto.MaxNumber = currentQuotaCount;
                    CourseLogDto.LogonId = course.LogonId;
                    CourseLogDto.Title = course.Title;
                    CourseLogDto.CreatedUserId = createdUserId;
                    CourseLogDto.CreateDate = DateTime.Now;
                    CourseLogDto.CourseDescriptionId = course.CourseDescriptionId;
                    CourseLogDto.SectionDepartmentId = course.SectionDepartmentId;
                    CourseLogDto.ClassTimeId = course.ClassTimeId;
                    CourseLogDto.ClassroomId = course.ClassroomId;
                    CourseLogDto.ApplicationUserId = course.ApplicationUserId;
                    CourseLogDto.CourseId = courseID;
                    db.CourseLog.Add(CourseLogDto);
                    db.SaveChanges();

                    Message = studentsLogonIdOption.Count() + "位學生選課成功,並且系統自動增加" + courseSubjectNumber + "課程上限人數~";
                }
                CourseStatus courseStatusDto = new CourseStatus();
                courseStatusDto.CourseId = courseID;
                courseStatusDto.CurrentQuota = currentQuotaCount;
                db.CourseStatus.Add(courseStatusDto);
                db.SaveChanges();

                

                return Json(new { message = Message });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }

        }

    }
}