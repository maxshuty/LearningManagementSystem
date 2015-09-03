using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampLMS.UI.Models
{
    public class StudentDashboardTableItem
    {
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public int EarnedPoints { get; set; }
        public int PointsPossible { get; set; }

        public decimal PercentGrade
        {
            get
            {
                decimal earn = (decimal) EarnedPoints;
                decimal poss = (decimal) PointsPossible;
                if (poss != 0)
                    return (earn/poss) * 100;
                else
                {
                    return 0;
                }
            }
        }
    }
}