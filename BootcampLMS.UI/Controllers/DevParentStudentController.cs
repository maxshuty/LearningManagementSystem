using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Controllers
{
    public class DevParentStudentController : Controller
    {
        ParentStudentRepo myParentStudentRepo = new ParentStudentRepo();
        UserProfileRepo myUserProfileRepo = new UserProfileRepo();

        public List<UserProfile> MyListUsers = new List<UserProfile>();
        public static List<string> MyListUserIds = new List<string>();


        public DevParentStudentController()
        {
            MyListUsers.Clear();
            MyListUserIds.Clear();

            MyListUsers = myUserProfileRepo.GetAll();
            foreach (var user in MyListUsers)
            {
                MyListUserIds.Add(user.UserId);
            }
        }

        // GET: DevParentStudent
        public ActionResult Index()
        {
            List<ParentStudent> myList = new List<ParentStudent>();
            myList = myParentStudentRepo.GetAll();

            return View(myList);
        }

        public ActionResult Add()
        {
            return View();
        }
        
        public ActionResult Delete(string pid, string sid)
        {
            myParentStudentRepo.Delete(pid, sid);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(ParentStudent parentstudent)
        {
            myParentStudentRepo.Add(parentstudent.ParentId, parentstudent.StudentId);
            return RedirectToAction("Index");
        }
    }
}