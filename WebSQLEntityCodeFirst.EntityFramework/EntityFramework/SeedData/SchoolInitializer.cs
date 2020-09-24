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
            CreateSchoolInitializer();
        }

        /**
         
         工程學群(A)
            電機工程學(A1) 電子工程學(A2)
         外語學群(B)
            英美語文學(B1)

         後八個數字為學生入門的順位

         **/

        private void CreateSchoolInitializer()
        {
            var students = new List<Student>
            {
            //電機工程學(A1)
            new Student{Name="陳	O豪",Gender=Gender.M,IDNo="A115940001",Class="3年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="林O洪",Gender=Gender.M,IDNo="A115940002",Class="3年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="林O傑",Gender=Gender.M,IDNo="A115940003",Class="3年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="陳O杰",Gender=Gender.M,IDNo="A115940070",Class="2年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="黃O牙",Gender=Gender.M,IDNo="A115940071",Class="2年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="陳	O尾",Gender=Gender.M,IDNo="A115940151",Class="2年2班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="李O怡",Gender=Gender.F,IDNo="A115940152",Class="2年2班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="周O倫",Gender=Gender.M,IDNo="A115940201",Class="1年1班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2011-09-01")},
            new Student{Name="林O瑋",Gender=Gender.M,IDNo="A115940241",Class="1年2班",Department="電機工程學",EnrollmentDate=DateTime.Parse("2011-09-01")},
            
            //電子工程學(A2)
            new Student{Name="陳O漢",Gender=Gender.M,IDNo="A215940001",Class="3年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="黃O如",Gender=Gender.F,IDNo="A215940002",Class="3年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="黃O賢",Gender=Gender.M,IDNo="A215940003",Class="3年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="林O憲",Gender=Gender.M,IDNo="A215940070",Class="2年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="吳O豪",Gender=Gender.M,IDNo="A215940071",Class="2年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="林O豪",Gender=Gender.M,IDNo="A215940151",Class="2年2班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="林O如",Gender=Gender.F,IDNo="A215940152",Class="2年2班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="陳	O豪",Gender=Gender.M,IDNo="A215940201",Class="1年1班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2011-09-01")},
            new Student{Name="黃O霖",Gender=Gender.M,IDNo="A215940241",Class="1年2班",Department="電子工程學",EnrollmentDate=DateTime.Parse("2011-09-01")},

            //英美語文學(B1)
            new Student{Name="李O怡",Gender=Gender.F,IDNo="B115940001",Class="3年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="吳O璇",Gender=Gender.F,IDNo="B115940002",Class="3年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="陳O雯",Gender=Gender.F,IDNo="B115940003",Class="3年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2009-09-01")},
            new Student{Name="林O琳",Gender=Gender.F,IDNo="B115940070",Class="2年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="黃O安",Gender=Gender.F,IDNo="B115940071",Class="2年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="陳O昌",Gender=Gender.M,IDNo="B115940151",Class="2年2班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="朴O慧",Gender=Gender.F,IDNo="B115940152",Class="2年2班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2010-09-01")},
            new Student{Name="李O君",Gender=Gender.F,IDNo="B115940201",Class="1年1班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2011-09-01")},
            new Student{Name="陳	O豪",Gender=Gender.M,IDNo="B115940241",Class="1年2班",Department="英美語文學",EnrollmentDate=DateTime.Parse("2011-09-01")}
            };

            students.ForEach(s => _context.Student.Add(s));
            _context.SaveChanges();
            var courses = new List<Course>
            {
            new Course{Subject="電路學",Title="Chemistry",Credits=5,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="電子學",Title="Microeconomics",Credits=5,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="微積分",Title="Macroeconomics",Credits=5,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="資料結構",Title="Calculus",Credits=5,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="英語",Title="Trigonometry",Credits=5,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="音樂欣賞",Title="Composition",Credits=3,Quota=50,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="籃球課",Title="Trigonometry",Credits=3,Quota=33,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")},
            new Course{Subject="電影欣賞",Title="Literature",Credits=3,Quota=50,CurrentQuota=0,CreateDate=DateTime.Parse("2001-09-01")}
            };
            courses.ForEach(s => _context.Course.Add(s));
            _context.SaveChanges();
            var electives = new List<Elective>
            {
            new Elective{StudentID=1,CourseID=1},
            new Elective{StudentID=2,CourseID=1},
            new Elective{StudentID=3,CourseID=2},
            new Elective{StudentID=1,CourseID=2},
            new Elective{StudentID=5,CourseID=6},
            new Elective{StudentID=2,CourseID=6},
            new Elective{StudentID=11,CourseID=7},
            new Elective{StudentID=10,CourseID=8,},
            new Elective{StudentID=16,CourseID=5},
            new Elective{StudentID=16,CourseID=7},
            new Elective{StudentID=14,CourseID=7},
            new Elective{StudentID=22,CourseID=5},
            };
            electives.ForEach(s => _context.Elective.Add(s));
            _context.SaveChanges();
        }
    }
}
