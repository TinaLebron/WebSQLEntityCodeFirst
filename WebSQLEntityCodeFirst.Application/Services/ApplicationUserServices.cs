using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebSQLEntityCodeFirst.EntityFramework.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebSQLEntityCodeFirst.Application.Services
{
    public class ApplicationUserServices
    {
        //private static SchoolContext _context = new SchoolContext();

        //依據SIDNo, Password做確認,確認無誤會回傳true,有誤則false
        //Non-static Class https://www.tutorialsteacher.com/csharp/csharp-static
        public static bool IsLoginBySIDNoAndPassword(string sIDNo, string password)
        {
            SchoolContext _context = new SchoolContext();
            if (sIDNo == null || password == null) { return false; }
            var userCount = _context.ApplicationUser.Where(x => x.LogonId == sIDNo).Count();
            if (userCount <= 0) { return false; }
            var user = _context.ApplicationUser.FirstOrDefault(x => x.LogonId == sIDNo);
            var IsLogin = VerifyHashedPassword(user.PasswordHash, password);
            return IsLogin;

        }

        //加密與解密:https://codertw.com/%E5%89%8D%E7%AB%AF%E9%96%8B%E7%99%BC/204722/
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
                return false;
            byte[] numArray = Convert.FromBase64String(hashedPassword);
            if (numArray.Length != 49 || (int)numArray[0] != 0)
                return false;
            byte[] salt = new byte[16];
            Buffer.BlockCopy((Array)numArray, 1, (Array)salt, 0, 16);
            byte[] a = new byte[32];
            Buffer.BlockCopy((Array)numArray, 17, (Array)a, 0, 32);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 1000))
                bytes = rfc2898DeriveBytes.GetBytes(32);
            return ApplicationUserServices.ByteArraysEqual(a, bytes);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals((object)a, (object)b))
                return true;
            if (a == null || b == null || a.Length != b.Length)
                return false;
            bool flag = true;
            for (int index = 0; index < a.Length; index++)
                flag &= (int)a[index] == (int)b[index];
            return flag;
        }

        //排除停職留薪員工(2),離職員工(3),休學(5),離校學生(6),畢業生(7),退休(9)
        public static string ExcludeSpecialPersons(string sIDNo)
        {
            SchoolContext _context = new SchoolContext();
            string messages = null;
            var userState = _context.ApplicationUser.FirstOrDefault(x => x.LogonId == sIDNo).UserStateId;
            if (userState == 2 || userState == 3 || userState == 5 || userState == 6 || userState == 7 || userState == 9)
            {
                messages = "您登入的帳號身分不適合此系統使用,如有問題請洽詢學校(0x)8888888";
                return messages;
            }
            

            return messages;

        }

        public static void UserpasswordUpdate(string logonId,string password)
        {
            SchoolContext db = new SchoolContext();
            var user = db.ApplicationUser.FirstOrDefault(x => x.LogonId == logonId);

            user.PasswordHash = new PasswordHasher().HashPassword(password);
            db.SaveChanges();
        }

    }
}
