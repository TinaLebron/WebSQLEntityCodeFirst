using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string LogonId { get; set; } // 員工代碼
        public int Annual { get; set; } //上班年度
        public string Tel { get; set; } //聯絡電話(分機)
        public string ResearchAreas { get; set; } //專業領域
        public string JobTitle { get; set; } //職稱
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        public bool IsActive { get; set; } //不使用
        public int ClassroomId { get; set; } // 辨公室
        public int ApplicationUserId { get; set; } 

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Classroom Classroom { get; set; }



    }
}
