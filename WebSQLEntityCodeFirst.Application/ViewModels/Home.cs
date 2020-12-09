using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class Home
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActive { get; set; }

    }
}
