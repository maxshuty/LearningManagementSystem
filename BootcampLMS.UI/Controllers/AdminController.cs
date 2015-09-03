using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;
using BootcampLMS.UI.Models;
using Microsoft.AspNet.Identity;

namespace BootcampLMS.UI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        UserProfileRepo repo = new UserProfileRepo();
        IdentityRepo MyIdentityRepo = new IdentityRepo();

        public ActionResult Index()
        {
            var vm = repo.GetUnassignedUsers();

            return View(vm);
        }

        public ActionResult Details(string id)
        {
            UserProfile myUserProfile = repo.GetById(id);
            UserDetailsViewModel myViewModel = new UserDetailsViewModel();

            myViewModel.UserId = myUserProfile.UserId;
            myViewModel.FirstName = myUserProfile.FirstName;
            myViewModel.LastName = myUserProfile.LastName;
            myViewModel.GradeLevel = (GradeLevels)myUserProfile.GradeLevel;

            return View(myViewModel);
        }


        [HttpPost]
        public ActionResult SaveDetails(UserDetailsViewModel myViewModel)
        {
            UserProfile myUserProfile = repo.GetById(myViewModel.UserId);
            myUserProfile.FirstName = myViewModel.FirstName;
            myUserProfile.LastName = myViewModel.LastName;
            myUserProfile.GradeLevel = (int)myViewModel.GradeLevel;

            repo.Edit(myUserProfile);

            ApplicationDbContext db = new ApplicationDbContext();
            var ParentRole = (from r in db.Roles where r.Name.Contains("Parent") select r).FirstOrDefault();
            var StudentRole = (from r in db.Roles where r.Name.Contains("Student") select r).FirstOrDefault();
            var AdminRole = (from r in db.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
            var TeacherRole = (from r in db.Roles where r.Name.Contains("Teacher") select r).FirstOrDefault();

            UserRoleCreateOrDelete(ParentRole.Id, myViewModel.UserId, myViewModel.ParentRole);
            UserRoleCreateOrDelete(StudentRole.Id, myViewModel.UserId, myViewModel.StudentRole);
            UserRoleCreateOrDelete(TeacherRole.Id, myViewModel.UserId, myViewModel.TeacherRole);
            UserRoleCreateOrDelete(AdminRole.Id, myViewModel.UserId, myViewModel.AdminRole);

            return RedirectToAction("Index");
        }

        private void UserRoleCreateOrDelete(string roleid, string userid, bool hasRole)
        {
            if (hasRole)
                MyIdentityRepo.Add(roleid, userid);
            else
                MyIdentityRepo.Delete(roleid, userid);
        }

        public ActionResult SearchResults(UserSearchViewModel search)
        {
            AdminSearchRepo repo = new AdminSearchRepo();
            if (search.RoleName == null)
                return View(search);

            if (search.FirstName != null || search.FirstName != null || search.Email != null || search.RoleName != null)
                search.SearchResults = repo.SearchResults(search.LastName, search.FirstName, search.Email, search.RoleName);

            return View(search);
        }
    }
}