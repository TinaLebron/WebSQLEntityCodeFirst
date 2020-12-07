using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ChooseAClassroomLog
    {
        [Key]
        public int ID { get; set; }
        public string Remarks { get; set; }
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int? ClassTimeId { get; set; }
        public int? ClassroomId { get; set; }
        public int? CourseId { get; set; }
        public string LogonId { get; set; } // 員工/學號(帳號)
        public int? ApplicationUserId { get; set; }
        public int ChooseAClassroomId { get; set; }


        public virtual ClassTime ClassTime { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual Course Course { get; set; } //課程
        public virtual ChooseAClassroom ChooseAClassroom { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
