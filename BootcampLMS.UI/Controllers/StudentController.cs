using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.UI.Models;
using Microsoft.AspNet.Identity;

namespace BootcampLMS.UI.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            StudentDashboardVM vm = new StudentDashboardVM(User.Identity.GetUserId());
            return View(vm);
        }

        public ActionResult ViewGrades(int courseid, string userid)
        {
            AssignmentTrackerRepo myRepo = new AssignmentTrackerRepo();
            List<AssignmentViewModel> myAsses = myRepo.GetAssignments(userid, courseid);
            return View(myAsses);
        }
    }
}