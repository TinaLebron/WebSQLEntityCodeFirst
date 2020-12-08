using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class CourseDescription
    {
        [Key]
        public int ID { get; set; }
        public string Objectives { get; set; } //授課目標
        public string CourseOutline { get; set; } //授課大綱
        public string Textbooks { get; set; } //教科書
        public string ReferenceBooks { get; set; } //參考書籍
        public string Grading { get; set; } //成績考核準則
        public string Schedule { get; set; } //授課進度

        public virtual IList<Course> Course { get; set; }
        public virtual IList<CourseLog> CourseLog { get; set; }

    }
}
