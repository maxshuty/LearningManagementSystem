using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
   public class Assignment
    {
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public int PointsPossible { get; set; }
    }
}
