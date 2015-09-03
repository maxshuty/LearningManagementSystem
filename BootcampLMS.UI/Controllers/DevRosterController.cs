using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Controllers
{
    public class DevRosterController : Controller
    {
        RosterRepo myRosterRepo = new RosterRepo();
        CourseRepo myCourseRepo = new CourseRepo();
        UserProfileRepo myUserRepo = new UserProfileRepo();

        List<Course> allCourses = new List<Course>();
        public static List<int> AllCourseIds = new List<int>();

        List<UserProfile> allUsers = new List<UserProfile>();
        public static List<string> AllUserIds = new List<string>();

        public DevRosterController()
        {
            allCourses.Clear();
            AllCourseIds.Clear();
            allCourses = myCourseRepo.GetAll();

            foreach (var course in allCourses)
            {
                AllCourseIds.Add(course.CourseId);
            }

            allUsers.Clear();
            AllUserIds.Clear();
            allUsers = myUserRepo.GetAll();

            foreach (var user in allUsers)
            {
                AllUserIds.Add(user.UserId);
            }
        }

        // GET: Roster
        public ActionResult Index()
        {
            List<Roster> myList = new List<Roster>();
            myList = myRosterRepo.GetAll();
            return View(myList);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            myRosterRepo.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var myRoster = myRosterRepo.GetById(id);

            return View(myRoster);
        }

        [HttpPost]
        public ActionResult Edit(Roster myRoster)
        {
            myRosterRepo.Edit(myRoster);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(Roster myRoster)
        {
            myRosterRepo.Add(myRoster);
            return RedirectToAction("Index");
        }
    }
}