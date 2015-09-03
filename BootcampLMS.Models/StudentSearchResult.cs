using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootcampLMS.Models
{
    public class StudentSearchResult
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? GradeLevel { get; set; }
    }
}
