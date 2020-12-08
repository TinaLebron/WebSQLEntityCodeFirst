using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class CourseStatus
    {

        [Key]
        public int ID { get; set; }
        public int CourseId { get; set; }
        [Required]
        public int CurrentQuota { get; set; } //目前名額


        public virtual Course Course { get; set; }

    }
}
