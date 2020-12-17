using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class SectionDepartmentList
    {

        public string Section { get; set; } //部別ex:大學部
        public string[] Department { get; set; } //科系

    }
}
