using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; } //性別
        [Required]
        public string IDNo { get; set; } //學號
        public string Class { get; set; } //班級
        public string Department { get; set; } //科系
        public DateTime EnrollmentDate { get; set; } //入學時間

        public virtual IList<Elective> Elective { get; set; }
    }
}
