using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseListsByCourseWork
    {
        public int CourseId { get; set; }
        public string SubjectNumber { get; set; }
        public string Subject { get; set; } //科目
        public int Credits { get; set; }
        public string TimeLocation { get; set; } //ex: 星期一(2節~4節) T20101
        public bool IsCourseLists { get; set; }
        public string Week { get; set; }
        public int FTime { get; set; }
        public int ETime { get; set; }

    }
}
