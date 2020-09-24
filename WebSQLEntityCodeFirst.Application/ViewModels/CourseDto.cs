using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseDto
    {
        [Key]
        public int CourseID { get; set; }
        [Required]
        public string Subject { get; set; } //科目
        public string Title { get; set; } //內容
        [Required]
        public int Credits { get; set; } //學分
        [Required]
        public int Quota { get; set; } //名額
        [Required]
        public int CurrentQuota { get; set; } //目前名額
        public DateTime CreateDate { get; set; } //建立日期

        public virtual IList<Elective> Elective { get; set; }
    }
}
