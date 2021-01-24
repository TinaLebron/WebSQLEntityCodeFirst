using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class CourseTimePerSemester
    {
        [Key]
        public int ID { get; set; }
        public int StartingSchoolYear { get; set; } //開課學年
        public Semester Semester { get; set; } //開課學期
        public int Grade { get; set; } //年級
        public DateTime SignupBeginDate { get; set; } //開始開放選課
        public DateTime SignupEndDate { get; set; } //結束開放選課
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        public string Section { get; set; }

        

    }
}
