using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class ApplicationUserRoles
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CreatedUserId { get; set; } //建立者
        [Required]
        public DateTime? CreateDate { get; set; } //建立時間

    }
}
