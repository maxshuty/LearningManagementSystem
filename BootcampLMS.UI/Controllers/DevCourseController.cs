using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Controllers
{
    public class DevCourseController : Controller
    {
        BootcampLMS.Data.Repositories.UserProfileRepo myRepo = new UserProfileRepo();
        CourseRepo myCourseRepo = new CourseRepo();

        public static List<string> AllTeacherIds = new List<string>();
        public List<UserProfile> AllUsers = new List<UserProfile>();

        public DevCourseController()
        {
            AllUsers.Clear();
            AllTeacherIds.Clear();

            AllUsers = myRepo.GetAll();
            foreach (var user in AllUsers)
            {
                AllTeacherIds.Add(user.UserId);
            }
        }

        // GET: DevCourse
        public ActionResult Index()
        {
            List<Course> myList = new List<Course>();
            myList = myCourseRepo.GetAll();

            return View(myList);
        }

        public ActionResult AddCourse()
        {
           return View();
        }

        [HttpPost]
        public ActionResult AddCourse(Course myCourse)
        {
            
            myCourseRepo.Add(myCourse);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            myCourseRepo.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var myCourse = myCourseRepo.GetById(id);
            
            return View(myCourse);
        }

        [HttpPost]
        public ActionResult Edit(Course myCourse)
        {
            myCourseRepo.Edit(myCourse);

            return RedirectToAction("Index");
        }
    }
}