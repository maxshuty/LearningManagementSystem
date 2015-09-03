using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;
using BootcampLMS.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BootcampLMS.UI.Controllers
{
    public class DevController : Controller
    {
        public UserProfileRepo myRepo = new UserProfileRepo();
        public IdentityRepo MyIdentityRepo = new IdentityRepo();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Dev
        public ActionResult Index()
        {
            
            var myList = myRepo.GetAll();
            return View(myList);
        }

        public ActionResult Details(string id)
        {
            UserProfile myUserProfile = myRepo.GetById(id);
            UserDetailsViewModel myViewModel = new UserDetailsViewModel();

            myViewModel.UserId = myUserProfile.UserId;
            myViewModel.FirstName = myUserProfile.FirstName;
            myViewModel.LastName = myUserProfile.LastName;
            myViewModel.GradeLevel = (GradeLevels) myUserProfile.GradeLevel;

            return View(myViewModel);
        }

        [HttpPost]
        public ActionResult SaveDetails(UserDetailsViewModel myViewModel)
        {
            UserProfile myUserProfile = myRepo.GetById(myViewModel.UserId);
            myUserProfile.FirstName = myViewModel.FirstName;
            myUserProfile.LastName = myViewModel.LastName;
            myUserProfile.GradeLevel = (int) myViewModel.GradeLevel;

            myRepo.Edit(myUserProfile);

            UserRoleCreateOrDelete("2a366a11-f079-4d01-99b9-d2df71668f79", myViewModel.UserId, myViewModel.ParentRole);
            UserRoleCreateOrDelete("10a747a1-296c-43a2-bcd7-be235735d2d1", myViewModel.UserId, myViewModel.StudentRole);
            UserRoleCreateOrDelete("2305a927-739d-4e43-a7ba-d98b775a3536", myViewModel.UserId, myViewModel.TeacherRole);
            UserRoleCreateOrDelete("40816bfe-82ca-4aba-81d6-121393648f0a", myViewModel.UserId, myViewModel.AdminRole);
            
            return RedirectToAction("Index");
        }

        private void UserRoleCreateOrDelete(string roleid, string userid, bool hasRole)
        {
            if (hasRole)
                MyIdentityRepo.Add(roleid, userid);
            else
                MyIdentityRepo.Delete(roleid, userid);
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(UserProfile myProfile)
        {
            
            myRepo.Edit(myProfile);

            return RedirectToAction("Index");
        }


        public ActionResult EditUserById(string id)
        {
           
            UserProfile myUser = myRepo.GetById(id);

            return View("EditUser", myUser);
        }

        public ActionResult Delete(string id)
        {
            
            myRepo.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddUser(UserProfile myUserProfile)
        {
            var user = new ApplicationUser { UserName = myUserProfile.Email, Email = myUserProfile.Email };
            await UserManager.CreateAsync(user, "Password1!");

            myUserProfile.UserId = user.Id;

            myRepo.Add(myUserProfile);

            return RedirectToAction("Index");
        }
    }
}