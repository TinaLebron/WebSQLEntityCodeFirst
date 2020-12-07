using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ElectiveLog
    {

        [Key]
        public int ID { get; set; }
        public string Remarks { get; set; }
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int? ClassTimeId { get; set; }
        public int? ClassroomId { get; set; }
        public int? CourseId { get; set; }
        public string LogonId { get; set; } // 只有學號(帳號)
        public int? StudentId { get; set; }
        public int ElectiveId { get; set; }


        public virtual ClassTime ClassTime { get; set; } 
        public virtual Classroom Classroom { get; set; } 
        public virtual Course Course { get; set; } //課程
        public virtual Student Student { get; set; }
        public virtual Elective Elective { get; set; }

    }
}
