using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Application.Services
{
    public class NewsServices
    {
        public static Home GetSubjectAndContents()
        {
            SchoolContext _context = new SchoolContext();
            var permissionsDtoList = new Home();
            var home = _context.Home.Where(x=>x.IsActive == true).OrderByDescending(x=>x.CreateDate).First();

            Home homesDto = new Home
            {
                ID = home.ID,
                Subject = home.Subject,
                Contents = home.Contents,
                CreateUser = home.CreateUser,
                CreateDate = home.CreateDate,
                IsActive = home.IsActive,
            };
            

            return homesDto;

        }
        
        public static void UpdateHomeInfo(string subject, string contents, int userId)
        {
            SchoolContext _context = new SchoolContext();

            WebSQLEntityCodeFirst.Core.DataModels.Home homesDto = new WebSQLEntityCodeFirst.Core.DataModels.Home();
            homesDto.Subject = subject;
            homesDto.Contents = contents;
            homesDto.CreateUser = userId;
            homesDto.CreateDate = DateTime.Now;
            homesDto.IsActive = true;

            _context.Home.Add(homesDto);
            _context.SaveChanges();

        }
    }
}
