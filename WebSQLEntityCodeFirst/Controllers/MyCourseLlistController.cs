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
    public class MyCourseLlistController : Controller
    {
        // GET: MyCourseLlist
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMyCourseLlist()
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<CourseInQuiryInfos> courseInQuiryInfosList = new List<CourseInQuiryInfos>();
                List<UserSchedulesDto> userSchedulesDtoList = new List<UserSchedulesDto>();
                List<CourseToUserSchedulesDto> courseDtoList = new List<CourseToUserSchedulesDto>();
                var courses = db.Course.Where(x => x.IsActive == true).OrderByDescending(x => x.CourseID).ToList();
                int startingSchoolYear = courses[0].StartingSchoolYear;
                var semester = courses[0].Semester;
                string schoolSemester = (semester == Core.Enums.Semester.F) ? "第一學期" : "第二學期";
                string loginId = Session["sIDNo"].ToString();

                courseInQuiryInfosList = GetCourseInQuiryInfosList(startingSchoolYear, schoolSemester, loginId);

                var Electives = db.Elective.Where(x => x.LogonId == loginId
                        && x.StartingSchoolYear == startingSchoolYear && x.Semester == semester).ToList();

                foreach (var e in Electives)
                {
                    var FTime = e.Course.ClassTime.ClassPeriod;
                    var ETime = (e.Course.NumberOfHours + e.Course.ClassTime.ClassPeriod) - 1;
                    for (int i = FTime; i <= ETime; i++)
                    {

                        var time = new string[] { "07:10~08:00", "08:10~09:00", "09:10~10:00", "10:10~11:00", "11:10~12:00"
                        , "12:00~13:10", "13:20~14:10","14:20~15:10","15:20~16:10","16:20~17:10","17:20~18:10","18:20~19:10"
                        ,"19:15~20:05","20:10~21:00","21:05~21:55" };
                        CourseToUserSchedulesDto courseDto = new CourseToUserSchedulesDto();
                        courseDto.CourseID = e.Course.CourseID;
                        courseDto.Subject = e.Course.Subject;
                        courseDto.SubjectNumber = e.Course.SubjectNumber;
                        courseDto.SchoolNumber = (e.Course.Classroom == null) ? "不指定教室" : e.Course.Classroom.SchoolNumber;
                        courseDto.Time = time[i];
                        courseDto.ClassPeriod = i;
                        courseDto.Week = e.Course.ClassTime.Week;

                        courseDtoList.Add(courseDto);

                    }
                }


                for (int i = 0; i < 15; i++)
                {
                    UserSchedulesDto userSchedulesDto = new UserSchedulesDto();
                    var week = new string[] { "一", "二", "三", "四", "五", "六", "日" };
                    var time = new string[] { "07:10~08:00", "08:10~09:00", "09:10~10:00", "10:10~11:00", "11:10~12:00"
                        , "12:00~13:10", "13:20~14:10","14:20~15:10","15:20~16:10","16:20~17:10","17:20~18:10","18:20~19:10"
                        ,"19:15~20:05","20:10~21:00","21:05~21:55" };

                    var semester1 = (semester == Core.Enums.Semester.F) ? "1" : "2";
                    userSchedulesDto.SchoolSemester = startingSchoolYear + semester1;
                    userSchedulesDto.ClassPeriod = i;
                    userSchedulesDto.Time = time[i];
                    userSchedulesDto.SubjectNumber1 = "";
                    userSchedulesDto.Subject1 = "";
                    userSchedulesDto.SchoolNumber1 = "";
                    userSchedulesDto.SubjectNumber2 = "";
                    userSchedulesDto.Subject2 = "";
                    userSchedulesDto.SchoolNumber2 = "";
                    userSchedulesDto.SubjectNumber3 = "";
                    userSchedulesDto.Subject3 = "";
                    userSchedulesDto.SchoolNumber3 = "";
                    userSchedulesDto.SubjectNumber4 = "";
                    userSchedulesDto.Subject4 = "";
                    userSchedulesDto.SchoolNumber4 = "";
                    userSchedulesDto.SubjectNumber5 = "";
                    userSchedulesDto.Subject5 = "";
                    userSchedulesDto.SchoolNumber5 = "";
                    userSchedulesDto.SubjectNumber6 = "";
                    userSchedulesDto.Subject6 = "";
                    userSchedulesDto.SchoolNumber6 = "";
                    userSchedulesDto.SubjectNumber7 = "";
                    userSchedulesDto.Subject7 = "";
                    userSchedulesDto.SchoolNumber7 = "";

                    for (int j = 0; j < week.Count(); j++)
                    {
                        var week1 = week[j];
                        var userCoursesCount = courseDtoList.Where(x => x.Week == week1 && (x.ClassPeriod == i)).Count();

                        if (userCoursesCount != 0)
                        {
                            var courseDtoList1 = courseDtoList.FirstOrDefault(x => x.Week == week1 && x.ClassPeriod == i);

                            if (j == 0)
                            {
                                userSchedulesDto.SubjectNumber1 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject1 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber1 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 1)
                            {
                                userSchedulesDto.SubjectNumber2 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject2 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber2 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 2)
                            {
                                userSchedulesDto.SubjectNumber3 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject3 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber3 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 3)
                            {
                                userSchedulesDto.SubjectNumber4 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject4 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber4 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 4)
                            {
                                userSchedulesDto.SubjectNumber5 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject5 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber5 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 5)
                            {
                                userSchedulesDto.SubjectNumber6 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject6 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber6 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 6)
                            {
                                userSchedulesDto.SubjectNumber7 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject7 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber7 = courseDtoList1.SchoolNumber;
                            }
                        }
                    }

                    userSchedulesDtoList.Add(userSchedulesDto);


                }


                return Json(new { schoolYear = startingSchoolYear, semester = schoolSemester, courseInQuiryInfos = courseInQuiryInfosList, schedules = userSchedulesDtoList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        [HttpPost]
        public JsonResult MyCourseLlistSearch(int schoolYear, string semester)
        {
            try
            {
                SchoolContext db = new SchoolContext();
                List<CourseInQuiryInfos> courseInQuiryInfosList = new List<CourseInQuiryInfos>();
                List<UserSchedulesDto> userSchedulesDtoList = new List<UserSchedulesDto>();
                List<CourseToUserSchedulesDto> courseDtoList = new List<CourseToUserSchedulesDto>();
                string loginId = Session["sIDNo"].ToString();
                var schoolSemester = (semester == "第一學期") ? Core.Enums.Semester.F : Core.Enums.Semester.S;

                courseInQuiryInfosList = GetCourseInQuiryInfosList(schoolYear, semester, loginId);

                var Electives = db.Elective.Where(x => x.LogonId == loginId
                        && x.StartingSchoolYear == schoolYear && x.Semester == schoolSemester).ToList();

                foreach (var e in Electives)
                {
                    var FTime = e.Course.ClassTime.ClassPeriod;
                    var ETime = (e.Course.NumberOfHours + e.Course.ClassTime.ClassPeriod) - 1;
                    for (int i = FTime; i <= ETime; i++)
                    {

                        var time = new string[] { "07:10~08:00", "08:10~09:00", "09:10~10:00", "10:10~11:00", "11:10~12:00"
                        , "12:00~13:10", "13:20~14:10","14:20~15:10","15:20~16:10","16:20~17:10","17:20~18:10","18:20~19:10"
                        ,"19:15~20:05","20:10~21:00","21:05~21:55" };
                        CourseToUserSchedulesDto courseDto = new CourseToUserSchedulesDto();
                        courseDto.CourseID = e.Course.CourseID;
                        courseDto.Subject = e.Course.Subject;
                        courseDto.SubjectNumber = e.Course.SubjectNumber;
                        courseDto.SchoolNumber = (e.Course.Classroom == null) ? "不指定教室" : e.Course.Classroom.SchoolNumber;
                        courseDto.Time = time[i];
                        courseDto.ClassPeriod = i;
                        courseDto.Week = e.Course.ClassTime.Week;

                        courseDtoList.Add(courseDto);

                    }
                }

                for (int i = 0; i < 15; i++)
                {
                    UserSchedulesDto userSchedulesDto = new UserSchedulesDto();
                    var week = new string[] { "一", "二", "三", "四", "五", "六", "日" };
                    var time = new string[] { "07:10~08:00", "08:10~09:00", "09:10~10:00", "10:10~11:00", "11:10~12:00"
                        , "12:00~13:10", "13:20~14:10","14:20~15:10","15:20~16:10","16:20~17:10","17:20~18:10","18:20~19:10"
                        ,"19:15~20:05","20:10~21:00","21:05~21:55" };

                    var semester1 = (semester == "第一學期") ? "1" : "2";
                    userSchedulesDto.SchoolSemester = schoolYear + semester1;
                    userSchedulesDto.ClassPeriod = i;
                    userSchedulesDto.Time = time[i];
                    userSchedulesDto.SubjectNumber1 = "";
                    userSchedulesDto.Subject1 = "";
                    userSchedulesDto.SchoolNumber1 = "";
                    userSchedulesDto.SubjectNumber2 = "";
                    userSchedulesDto.Subject2 = "";
                    userSchedulesDto.SchoolNumber2 = "";
                    userSchedulesDto.SubjectNumber3 = "";
                    userSchedulesDto.Subject3 = "";
                    userSchedulesDto.SchoolNumber3 = "";
                    userSchedulesDto.SubjectNumber4 = "";
                    userSchedulesDto.Subject4 = "";
                    userSchedulesDto.SchoolNumber4 = "";
                    userSchedulesDto.SubjectNumber5 = "";
                    userSchedulesDto.Subject5 = "";
                    userSchedulesDto.SchoolNumber5 = "";
                    userSchedulesDto.SubjectNumber6 = "";
                    userSchedulesDto.Subject6 = "";
                    userSchedulesDto.SchoolNumber6 = "";
                    userSchedulesDto.SubjectNumber7 = "";
                    userSchedulesDto.Subject7 = "";
                    userSchedulesDto.SchoolNumber7 = "";

                    for (int j = 0; j < week.Count(); j++)
                    {
                        var week1 = week[j];
                        var userCoursesCount = courseDtoList.Where(x => x.Week == week1 && (x.ClassPeriod == i)).Count();

                        if (userCoursesCount != 0)
                        {
                            var courseDtoList1 = courseDtoList.FirstOrDefault(x => x.Week == week1 && x.ClassPeriod == i);

                            if (j == 0)
                            {
                                userSchedulesDto.SubjectNumber1 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject1 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber1 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 1)
                            {
                                userSchedulesDto.SubjectNumber2 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject2 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber2 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 2)
                            {
                                userSchedulesDto.SubjectNumber3 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject3 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber3 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 3)
                            {
                                userSchedulesDto.SubjectNumber4 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject4 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber4 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 4)
                            {
                                userSchedulesDto.SubjectNumber5 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject5 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber5 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 5)
                            {
                                userSchedulesDto.SubjectNumber6 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject6 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber6 = courseDtoList1.SchoolNumber;
                            }
                            else if (j == 6)
                            {
                                userSchedulesDto.SubjectNumber7 = courseDtoList1.SubjectNumber;
                                userSchedulesDto.Subject7 = courseDtoList1.Subject;
                                userSchedulesDto.SchoolNumber7 = courseDtoList1.SchoolNumber;
                            }
                        }
                    }

                    userSchedulesDtoList.Add(userSchedulesDto);


                }




                return Json(new { courseInQuiryInfos = courseInQuiryInfosList, schedules = userSchedulesDtoList });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, error = false });
            }
        }

        public List<CourseInQuiryInfos> GetCourseInQuiryInfosList(int schoolYear, string semester, string loginId)
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

            var Electives = db.Elective.Where(x => x.LogonId == loginId && x.StartingSchoolYear == schoolYear && x.Semester == semester1).OrderBy(x => x.ID).ToList();
            



            foreach (var e in Electives)
            {
                string requiredElective = "";
                string status1 = GetStatusProgress(e.Course.SignupBeginDate, e.Course.SignupEndDate);

                if (e.Course.RequiredElective == RequiredElective.E)
                {
                    requiredElective = "選";
                }
                else if (e.Course.RequiredElective == RequiredElective.R)
                {
                    requiredElective = "必";
                }

                string schoolNumber = "";

                if (e.Course.ClassroomId != null)
                {
                    schoolNumber = e.Course.Classroom.SchoolNumber;
                }

                CourseInQuiryInfos courseInQuiryInfos = new CourseInQuiryInfos
                {
                    CourseID = e.Course.CourseID,
                    Subject = e.Course.Subject,
                    SubjectNumber = e.Course.SubjectNumber,
                    Credits = e.Credits,
                    ClassYear = schoolYear + " " + semester,
                    GradeClass = e.Course.SectionDepartment.Section + e.Course.SectionDepartment.DepartmentAbbreviation + e.Course.Grade + e.Course.Class,
                    RequiredElective = requiredElective,
                    TimeLocation = GetTimeLocation(e.Course.ClassTimeId, e.Course.NumberOfHours, schoolNumber),
                    Instructor = e.Course.ApplicationUser.UserName,
                    MaximumNum = e.Course.MinNumber + "-" + e.Course.MaxNumber,
                    CurrentNum = GetCurrentNum(e.Course.CourseID),
                    ApplicationUserId = e.Course.ApplicationUserId,
                    Status = status1
                };
                courseInQuiryInfosList.Add(courseInQuiryInfos);




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

    }
}