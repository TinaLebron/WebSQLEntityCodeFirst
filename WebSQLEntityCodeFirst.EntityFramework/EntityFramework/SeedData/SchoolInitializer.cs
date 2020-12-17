using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;
using WebSQLEntityCodeFirst.Core.Enums;

namespace WebSQLEntityCodeFirst.EntityFramework.EntityFramework.SeedData
{
    public class SchoolInitializer
    {
        private readonly SchoolContext _context;

        public SchoolInitializer(SchoolContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //CreateSchoolInitializer();
        }
        
        
    }
}
