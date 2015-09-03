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
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult Index()
        {
            ParentDashboardVM vm = new ParentDashboardVM(User.Identity.GetUserId());
            return View(vm);
        }

        public ActionResult Grades(string userid)
        {
            StudentDashboardVM vm = new StudentDashboardVM(userid);
            return View(vm);
        }

        public ActionResult Assignments(int courseid, string userid)
        {
            AssignmentTrackerRepo myRepo = new AssignmentTrackerRepo();
            List<AssignmentViewModel> myAsses = myRepo.GetAssignments(userid, courseid);
            return View(myAsses);
        }
    }
}