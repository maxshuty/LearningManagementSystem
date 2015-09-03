using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Models
{
    public class CourseVM
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


        public AnalyticsModel MyAnalModel
        {
            get
            {
                CourseRepo myCourseRepo = new CourseRepo();
                return new AnalyticsModel(myCourseRepo.GetGrades(CourseId));
            }

        }

        public CourseVM(Course myCourse)
        {
            CourseId = myCourse.CourseId;
            TeacherId = myCourse.TeacherId;
            Name = myCourse.Name;
            Department = myCourse.Department;
            CourseDescription = myCourse.CourseDescription;
            StartDate = myCourse.StartDate;
            EndDate = myCourse.EndDate;
            GradeLevel = myCourse.GradeLevel;
            IsArchived = myCourse.IsArchived;
        }
}
}
