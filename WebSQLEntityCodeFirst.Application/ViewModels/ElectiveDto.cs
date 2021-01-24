using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class ElectiveDto
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        public virtual Course Course { get; set; } //課程
        public virtual Student Student { get; set; }
    }
}
