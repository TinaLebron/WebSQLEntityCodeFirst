using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ClassTime
    {
        [Key]
        public int ID { get; set; }
        public string TimePeriod { get; set; } //時段 ex: 日間時間,夜間時間
        public string Time { get; set; } //上課時間
        public int ClassPeriod { get; set; } //節次
        public string Week { get; set; } //星期 ex: 一~日

        
        public virtual IList<ChooseAClassroom> ChooseAClassroom { get; set; }
        public virtual IList<Course> Course { get; set; }
        public virtual IList<CourseLog> CourseLog { get; set; }
        public virtual IList<ChooseAClassroomLog> ChooseAClassroomLog { get; set; }


    }
}
