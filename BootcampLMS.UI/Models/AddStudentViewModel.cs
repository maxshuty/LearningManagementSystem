using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Models
{
    public class AddStudentViewModel
    {
        public int CourseId { get; set; }
        public string LastName { get; set; }
        public int? GradeLevel { get; set; }
        public List<StudentSearchResult> SearchResults { get; set; }

        public AddStudentViewModel(int courseid)
        {
            CourseId = courseid;
            CourseRepo courseRepo = new CourseRepo();
            SearchResults = courseRepo.GetStudentsNotInCourse(courseid);
        }
    }
}