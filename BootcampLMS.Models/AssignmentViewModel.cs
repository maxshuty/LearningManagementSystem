using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampLMS.UI.Models
{
    public class AssignmentViewModel
    {
        public string AssignmentName { get; set; }
        public string UserId { get; set; }
        public int EarnedPoints { get; set; }
        public int PointsPossible { get; set; }

        public decimal Grade
        {
            get
            {
                if (PointsPossible != 0)
                {
                  var actualGrade = (decimal) EarnedPoints/(decimal) PointsPossible;
                    return actualGrade*100;
                }
                return 0;
            }
        }

        public char GetLetterGrade()
        {
           
                if (Grade >= 90)
                {
                    return 'A';
                }
                else if (Grade >= 80 && Grade <= 89)
                {
                    return 'B';
                }
                else if (Grade >= 70 && Grade <= 79)
                {
                    return 'C';
                }
                else if (Grade >= 60 && Grade <= 69)
                {
                    return 'D';
                }
                else
                {
                    return 'F';
                }
        }
    }
}