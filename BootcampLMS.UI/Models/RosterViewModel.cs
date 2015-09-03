using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Models
{
    public class RosterViewModel
    {
        public Course MyCourse { get; set; }
        
        public List<RosterTableViewModel> RosterTableViewModelList { get; set; }

        public AddStudentViewModel AddStudent { get; set; }


        public RosterViewModel(int courseid)
        {
            AddStudent = new AddStudentViewModel(courseid);

            CourseRepo myCourseRepo = new CourseRepo();
            RosterRepo myRosterRepo = new RosterRepo();

           
            RosterTableViewModelList = myRosterRepo.GetRosterTableItem(courseid);
            MyCourse = myCourseRepo.GetById(courseid);
            
        }
    }
}