using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;
using BootcampLMS.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace BootcampLMS.UI.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        CourseRepo repo = new CourseRepo();
        UserProfileRepo userProfileRepo = new UserProfileRepo();
        AssignmentTrackerRepo assTrackRepo = new AssignmentTrackerRepo();

        public static List<UserProfile> AllTeachers = new List<UserProfile>();
        public static List<string> AllTeacherIds = new List<string>(); 

        public TeacherController()
        {
            AllTeachers.Clear();
            AllTeacherIds.Clear();

            AllTeachers = userProfileRepo.GetTeachers();
            foreach (var teacher in AllTeachers)
            {
                AllTeacherIds.Add(teacher.UserId);
            }
        }

        // GET: Teacher
        public ActionResult Index()
        {
            return GetDashboardView(false);
        }

        public ActionResult Archive()
        {
            return GetDashboardView(true);
        }

        private ActionResult GetDashboardView(bool archived)
        {
           string teacherId = User.Identity.GetUserId();

            var allCourses = repo.GetSummariesByTeacher(teacherId);

            TeacherDashboardViewModel vm = new TeacherDashboardViewModel();
            vm.Courses = allCourses.Where(c => c.IsArchived == archived).ToList();
            vm.Current = allCourses.Count(c => c.IsArchived);
            vm.Archived = allCourses.Count(c => c.IsArchived);
            return View("TeacherDashboard", vm);
        }

        
        public ActionResult Information(int id)
        {
            Course myCourse = repo.GetById(id);
            CourseVM myVM = new CourseVM(myCourse);
            return View(myVM);
        }

        public ActionResult Roster(int id)
        {
            RosterViewModel myVm = new RosterViewModel(id);
            return View(myVm);
        }

        public ActionResult DeleteStudent(int courseId, int rosterId)
        {
            RosterRepo myRosterRepo = new RosterRepo();
            myRosterRepo.Archive(rosterId);

            RosterViewModel myVm = new RosterViewModel(courseId);
            return View("Roster", myVm);
        }

        public ActionResult AddRoster(string id, int courseid)
        {
            RosterRepo myRosterRepo = new RosterRepo();
            Roster myRoster = new Roster();
            myRoster.UserId = id;
            myRoster.CourseId = courseid;
            myRosterRepo.Add(myRoster);

            RosterViewModel myVm = new RosterViewModel(courseid);
            return View("Roster", myVm);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Course newCourse)
        {
            newCourse.TeacherId = User.Identity.GetUserId();
            repo.Add(newCourse);
            return GetDashboardView(false);
        }

    
        public ActionResult AnalView(AnalyticsModel am)
        {
            return View(am);
        }

    
    }
}