using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class UserState
    {
        [Key]
        public int ID { get; set; }
        public string InSchoolState { get; set; } //在學校的狀況 ex: 在職,停職留薪,在校,休學...


        public virtual IList<ApplicationUser> ApplicationUser { get; set; }
        public virtual IList<Student> Student { get; set; }

    }
}
