using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Models
{
    public class StudentDashboardVM
    {
        public List<StudentDashboardTableItem> myTableItemList = new List<StudentDashboardTableItem>();

        public String UserId;
        public String Name;

        public StudentDashboardVM(string userid)
        {
            AssignmentTrackerRepo myAssTrackerRepo = new AssignmentTrackerRepo();
            UserProfileRepo myUserProfileRepo = new UserProfileRepo();

            List<UserProfile> users = myUserProfileRepo.GetAll();

            var profile = users.FirstOrDefault(u => u.UserId == userid);
            Name = profile.FirstName + " " + profile.LastName;

            UserId = userid;
            myTableItemList = myAssTrackerRepo.GetCourseGrades(userid);

        }
    }
}