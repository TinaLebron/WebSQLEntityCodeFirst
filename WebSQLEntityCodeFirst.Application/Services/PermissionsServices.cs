using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.Application.ViewModels;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;

namespace WebSQLEntityCodeFirst.Application.Services
{
    public class PermissionsServices
    {
        //private static SchoolContext _context = new SchoolContext();

        //管控權限
        public static List<PermissionsDto> GetPersonalPermissions(string LogonId)
        {
            SchoolContext _context = new SchoolContext();
            var permissionsDtoList = new List<PermissionsDto>();
            var user = _context.ApplicationUser.FirstOrDefault(x => x.LogonId == LogonId);
            var userRoles = _context.ApplicationUserRoles.FirstOrDefault(x => x.UserId == user.ID);
            var permissions = _context.Permissions.Where(x => x.RoleId == userRoles.RoleId);

            foreach (var permission in permissions)
            {
                PermissionsDto permissionsDto = new PermissionsDto
                {
                    ID = permission.ID,
                    Name = permission.Name,
                    Url = permission.Url,
                    IsGranted = permission.IsGranted,
                    CreatedUserId = permission.CreatedUserId,
                    CreateDate = permission.CreateDate,
                    RoleId = permission.RoleId,
                    MenuItemsId = permission.MenuItemsId
                };

                permissionsDtoList.Add(permissionsDto);

            }

            return permissionsDtoList;

        }
    }
}
