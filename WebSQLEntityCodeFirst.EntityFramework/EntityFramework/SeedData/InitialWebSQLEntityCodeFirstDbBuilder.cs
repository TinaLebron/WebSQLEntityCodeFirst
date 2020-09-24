using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.EntityFramework.EntityFramework.SeedData
{
    public class InitialWebSQLEntityCodeFirstDbBuilder
    {
        private readonly SchoolContext _context;

        public InitialWebSQLEntityCodeFirstDbBuilder(SchoolContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new SchoolInitializer(_context).Create();

            _context.SaveChanges();
        }
    }
}
