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

namespace BootcampLMS.UI.Models
{
    public class ParentDashboardVM
    {
        public List<UserProfile> myListOfKids = new List<UserProfile>();

        public String UserId;

        public ParentDashboardVM(string userid)
        {
            ParentStudentRepo myParentStudentRepo = new ParentStudentRepo();

            UserId = userid;
            myListOfKids = myParentStudentRepo.GetKidsByParent(userid);
        }
    }
}