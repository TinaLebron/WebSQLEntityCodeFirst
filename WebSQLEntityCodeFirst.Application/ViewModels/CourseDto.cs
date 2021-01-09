using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseDto
    {
        public int CourseID { get; set; }
        public string Subject { get; set; } //科目
        public string SubjectNumber { get; set; } //科目代碼
        public int Credits { get; set; } //學分
        public int StartingSchoolYear { get; set; } //開課學年
        public Semester Semester { get; set; } //開課學期
        public int Grade { get; set; } //年級
        public string Class { get; set; } //班級
        public RequiredElective RequiredElective { get; set; } //選必修(必修:0,選修:1)
        public int NumberOfHours { get; set; } //上課時數
        public int HoursInternship { get; set; } //實習時數
        public DateTime CourseDate { get; set; } //開始上課時間
        public DateTime SignupBeginDate { get; set; } //開始開放選課
        public DateTime SignupEndDate { get; set; } //結束開放選課
        public int MinNumber { get; set; } //最小數量
        public int MaxNumber { get; set; } //最大數量
        public string LogonId { get; set; } // 員工
        public string Title { get; set; } //內容
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        public bool IsActive { get; set; }
        public int CourseDescriptionId { get; set; }
        public int SectionDepartmentId { get; set; }
        public int? ClassTimeId { get; set; }
        public int? ClassroomId { get; set; }
        public int? ApplicationUserId { get; set; }

    }
}
