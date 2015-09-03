using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class Course
    {
       public int CourseId { get; set; }
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string CourseDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? GradeLevel { get; set; }
        public bool IsArchived { get; set; }

     }
}
