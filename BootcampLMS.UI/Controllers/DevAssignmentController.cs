using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Controllers
{
    public class DevAssignmentController : Controller
    {

        AssignmentRepo myAssignmentRepo = new AssignmentRepo();
        CourseRepo myCourseRepo = new CourseRepo();

        public static List<int> AllAssignmentIds = new List<int>();
        public List<Assignment> AllAssignments = new List<Assignment>();

        public static List<int> AllCourseIds = new List<int>();
        public List<Course> AllCourses = new List<Course>();

        public DevAssignmentController()
        {
            //Maybe in prod mode only have id lists in their matching controller, one list per controller
            AllAssignments.Clear();
            AllAssignmentIds.Clear();

            AllAssignments = myAssignmentRepo.GetAll();
            foreach (var assignment in AllAssignments)
            {
                AllAssignmentIds.Add(assignment.AssignmentId);
            }

            AllCourses.Clear();
            AllCourseIds.Clear();

            AllCourses = myCourseRepo.GetAll();
            foreach (var course in AllCourses)
            {
                AllCourseIds.Add(course.CourseId);
            }
        }

        // GET: DevAssignment
        public ActionResult Index()
        {
           List<Assignment> myAssignments = new List<Assignment>();
            myAssignments = myAssignmentRepo.GetAll();
            return View(myAssignments);
        }

        public ActionResult AddAssignment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAssignment(Assignment myAssignment)
        {
            myAssignmentRepo.Add(myAssignment);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            myAssignmentRepo.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var myAssignment = myAssignmentRepo.GetById(id);

            return View(myAssignment);
        }

        [HttpPost]
        public ActionResult Edit(Assignment myAssignment)
        {
            myAssignmentRepo.Edit(myAssignment);

            return RedirectToAction("Index");
        }
        
    }
}