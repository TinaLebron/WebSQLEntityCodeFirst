using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class GetCourseTimePerSemester
    {
        public int CourseTimePerSemesterID { get; set; }
        public string SchoolYearSemester { get; set; } //ex: 109學年度第二學期
        public string SchoolSystem { get; set; } //各學制 ex: 大學部
        public string CourseTime { get; set; } //選課時間 ex: 4年級(含以上) 2020/12/28 08:00 ~ 2021/1/8 17:00

    }
}
