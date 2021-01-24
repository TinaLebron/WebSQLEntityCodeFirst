using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ChooseAClassroom
    {
        [Key]
        public int ID { get; set; }
        public int StartingSchoolYear { get; set; } //開課學年
        public Semester Semester { get; set; } //開課學期
        public int NumberOfHours { get; set; } //上課時數
        public int? ClassTimeId { get; set; }
        public int? ClassroomId { get; set; }
        public int? CourseId { get; set; }
        public string LogonId { get; set; } // 員工/學號(帳號)
        public int ApplicationUserId { get; set; }
        public string Remarks { get; set; }



        public virtual ClassTime ClassTime { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual Course Course { get; set; } //課程
        public virtual ApplicationUser ApplicationUser { get; set; }
        //public virtual IList<ChooseAClassroomLog> ChooseAClassroomLog { get; set; }

    }
}
