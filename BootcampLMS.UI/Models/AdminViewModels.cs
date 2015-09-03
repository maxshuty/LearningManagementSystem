using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampLMS.UI.Models
{
    public enum GradeLevels
    {
        Kindergarten,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh,
        Eight,
        Freshman,
        Sophomore,
        Junior,
        Senior
    }

    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public GradeLevels GradeLevel { get; set; }
        public bool AdminRole { get; set; }
        public bool StudentRole { get; set; }
        public bool TeacherRole { get; set; }
        public bool ParentRole { get; set; }
    }
}