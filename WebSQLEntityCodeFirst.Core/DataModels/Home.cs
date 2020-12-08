using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Core.DataModels
{
    public class Home
    {
        [Key]
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActive { get; set; }
     
    }
}
