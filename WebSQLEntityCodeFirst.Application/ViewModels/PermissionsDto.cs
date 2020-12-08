using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class PermissionsDto
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Url { get; set; }
        [Required]
        public bool IsGranted { get; set; }
        public int CreatedUserId { get; set; } //建立者
        public DateTime? CreateDate { get; set; } //建立時間
        public int RoleId { get; set; }

        public int? MenuItemsId { get; set; }

        public virtual ApplicationRoles ApplicationRoles { get; set; }
        public virtual MenuItems MenuItems { get; set; }
    }
}
