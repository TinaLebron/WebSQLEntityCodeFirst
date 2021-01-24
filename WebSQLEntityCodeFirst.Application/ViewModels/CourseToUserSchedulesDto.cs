using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseToUserSchedulesDto
    {
        public int CourseID { get; set; }
        public string Subject { get; set; } //科目
        public string SubjectNumber { get; set; } //科目代碼
        public string SchoolNumber { get; set; } //學校代號 ex: T1->第一教學大樓(文科),T2->第二教學大樓(理科),A1->行政大樓
        public string Time { get; set; } //上課時間
        public int ClassPeriod { get; set; } //節次
        public string Week { get; set; } //星期 ex: 一~日
    }
}
