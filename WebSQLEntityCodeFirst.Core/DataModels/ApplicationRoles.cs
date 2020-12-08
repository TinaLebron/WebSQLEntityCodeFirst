using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ApplicationRoles
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int LastModifiedUserId { get; set; } //最後編輯的使用者
        public DateTime? LastModifyDate { get; set; } //最後編輯的時間
        [Required]
        public bool IsActive { get; set; } //不使用


        public virtual IList<Permissions> Permissions { get; set; }
    }
}
