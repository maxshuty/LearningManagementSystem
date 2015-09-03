using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class AnalyticsModel
    {
        public List<AnalyticsModelItem> MyItemses { get; set; }

        public int AStudents { get; set; }
        public int BStudents { get; set; }
        public int CStudents { get; set; }
        public int DStudents { get; set; }
        public int FStudents { get; set; }

        public AnalyticsModel(List<AnalyticsModelItem> myList )
        {
            MyItemses = myList;

            foreach (var item in MyItemses)
            {
                if (item.Grade >= 90)
                {
                    AStudents++;
                }
                else if (item.Grade >= 80 && item.Grade <= 89)
                {
                    BStudents++;
                }
                else if (item.Grade >= 70 && item.Grade <= 79)
                {
                    CStudents++;
                }
                else if (item.Grade >= 60 && item.Grade <= 69)
                {
                    DStudents++;
                }
                else
                {
                    FStudents++;
                }
            }
        }
    }
}
