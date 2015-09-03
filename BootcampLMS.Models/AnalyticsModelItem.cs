using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class AnalyticsModelItem
    {
        public int RosterId { get; set; }
        public int EarnedPointsTotal { get; set; }
        public int PointsPossible { get; set; }

        public int Grade
        {
            get
            {
                if (PointsPossible != 0)
                {
                    decimal percent = (decimal) EarnedPointsTotal/(decimal) PointsPossible;
                    decimal wtf = Math.Round(percent*100);
                    return (int) wtf;
                }
                return 0;
            }
        }
    }
}
