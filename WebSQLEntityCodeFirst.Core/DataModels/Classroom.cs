using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class Classroom
    {
        [Key]
        public int ID { get; set; }
        public string SchoolNumber { get; set; } //學校代號 ex: T1->第一教學大樓(文科),T2->第二教學大樓(理科),A1->行政大樓
        public string ClassroomNumber { get; set; } //教室門牌
        public string Location { get; set; } //地點
        public string Floor { get; set; } //樓層
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        [Required]
        public bool IsActive { get; set; } 


        public virtual IList<Employee> Employee { get; set; }
        public virtual IList<ChooseAClassroom> ChooseAClassroom { get; set; }
        public virtual IList<ChooseAClassroomLog> ChooseAClassroomLog { get; set; }
        public virtual IList<Course> Course { get; set; }
        public virtual IList<CourseLog> CourseLog { get; set; }


    }
}
