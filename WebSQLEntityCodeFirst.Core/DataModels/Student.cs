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
        [Required]
        public string LogonId { get; set; } // 員工/學號(帳號)
        public int Annual { get; set; } //入學學年
        public int Grade { get; set; } //年級
        public string Class { get; set; } //班級
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        public bool IsActive { get; set; } //不使用
        public int ApplicationUserId { get; set; }
        public int SectionDepartmentId { get; set; }
        public int? UserStateId { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual SectionDepartment SectionDepartment { get; set; }//部別系所
        public virtual UserState UserState { get; set; }
        public virtual IList<Elective> Elective { get; set; }
        
    }
}
