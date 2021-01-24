using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Core.DataModels;

namespace WebSQLEntityCodeFirst.EntityFramework.EntityFramework
{
    public class SchoolContext : IdentityDbContext<IdentityUser>
    {

        public SchoolContext() : base("SchoolContext")
        {
        }

        public DbSet<ApplicationRoles> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationUserRoles> ApplicationUserRoles { get; set; }
        public DbSet<IUser> IUser { get; set; }
        public DbSet<ChooseAClassroom> ChooseAClassroom { get; set; }
        public DbSet<ChooseAClassroomLog> ChooseAClassroomLog { get; set; }
        public DbSet<Classroom> Classroom { get; set; }
        public DbSet<ClassTime> ClassTime { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseLog> CourseLog { get; set; }
        public DbSet<CourseDescription> CourseDescription { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<Elective> Elective { get; set; }
        public DbSet<ElectiveLog> ElectiveLog { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<SectionDepartment> SectionDepartment { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<UserState> UserState { get; set; }
        public DbSet<Home> Home { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<CourseTimePerSemester> CourseTimePerSemester { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
