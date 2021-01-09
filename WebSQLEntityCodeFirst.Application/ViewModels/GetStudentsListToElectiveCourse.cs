using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class GetStudentsListToElectiveCourse
    {

        public string DepartmentAbbreviation { get; set; } //科系縮寫
        public List<StudentsInfo> StudentsInfos { get; set; }


    }

    public class StudentsInfo
    {

        public string LogonId { get; set; } // 學號(帳號)
        public string ClassLogonId { get; set; } // ex: 1A A115940002



    }
}
