using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSQLEntityCodeFirst.Application.ViewModels
{
    public class CourseListsByCartsByCourseWork
    {
        public int ElectiveId { get; set; }
        public int CourseId { get; set; }
        public string SubjectNumber { get; set; }
        public string Subject { get; set; } //科目
        public int Credits { get; set; }
        public string TimeLocation { get; set; } //ex: 星期一(2節~4節) T20101
        public string Status { get; set; } //未確認: 代表選的課程只有在購物車裡並沒有後續動作,選課成功: 代表選的課程已成功選入
        public string Week { get; set; }
        public int FTime { get; set; }
        public int ETime { get; set; }

    }
}
