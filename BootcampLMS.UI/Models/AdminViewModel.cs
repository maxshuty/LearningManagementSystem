using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BootcampLMS.Models;

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

    public class UserSearchViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<AdminSearchResult> SearchResults { get; set; }
    }


}