using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class Elective
    {
        [Key]
        public int ID { get; set; }
        public int StartingSchoolYear { get; set; } //開課學年
        public Semester Semester { get; set; } //開課學期
        [Required]
        public int Credits { get; set; } //學分
        public int CourseId { get; set; }
        public string LogonId { get; set; } // 只有學號(帳號)
        public int? StudentId { get; set; }

        public virtual Course Course { get; set; } //課程
        public virtual Student Student { get; set; }
        public virtual IList<ElectiveLog> ElectiveLog { get; set; }
        
    }
}
