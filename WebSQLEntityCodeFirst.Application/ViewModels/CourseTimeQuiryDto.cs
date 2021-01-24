using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseTimeQuiryDto
    {
        public int CourseTimePerSemesterID { get; set; }
        public string SchoolSystem { get; set; } //各學制 ex: 大學部
        public List<CourseTime> CourseTimeList { get; set; } 
    }

    public class CourseTime
    {
        public string CourseTimeString { get; set; } //選課時間 ex: 4年級(含以上) 2020/12/28 08:00
    }
}
