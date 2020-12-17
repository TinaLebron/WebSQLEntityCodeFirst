using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseInQuiryInfos
    {
        public int CourseID { get; set; }
        public string Subject { get; set; } //科目
        public string SubjectNumber { get; set; } //科目代碼
        public int Credits { get; set; } //學分
        public string GradeClass { get; set; } //開課系級
        public string RequiredElective { get; set; } //選必修
        public string TimeLocation { get; set; } //時間地點
        public string Instructor { get; set; } //教師
        public string MaximumNum { get; set; } //人數上限
        public int CurrentNum { get; set; } //目前人數
        public int? ApplicationUserId { get; set; }






    }
}
