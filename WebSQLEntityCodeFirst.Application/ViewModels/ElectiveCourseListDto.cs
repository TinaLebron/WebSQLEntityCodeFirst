using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class ElectiveCourseListDto
    {
        public int ElectivesID { get; set; }

        public int CourseID { get; set; }
        public string SubjectNumber { get; set; } //科目代碼

        public int? StudentId { get; set; }
        public string UserClass { get; set; } //ex: 大學部 資工系1A
        public string StudentLogonId { get; set; } // 員工/學號(帳號)
        public int Number { get; set; } //順位


    }
}
