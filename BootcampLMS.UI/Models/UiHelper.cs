using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.UI.Controllers;

namespace BootcampLMS.UI.Models
{
    public static class UiHelper
    {
        public static IEnumerable<SelectListItem> DropdownRoles
        {
            get;
            private set;
        }
        public static IEnumerable<SelectListItem> DropdownGradeLevels
        {
            get;
            private set;
        }

        public static IEnumerable<SelectListItem> Teachers
        {
            get; private set; 
        }

        static UiHelper()
        {
            DropdownRoles = new SelectListItem[]
{
new SelectListItem {Text = "Student", Value = "Student"},
new SelectListItem {Text = "Parent", Value = "Parent"},
new SelectListItem {Text = "Teacher", Value = "Teacher"}
};
            List<SelectListItem> teachers = new List<SelectListItem>();
            
            foreach (var teacher in TeacherController.AllTeachers)
            {
                teachers.Add(new SelectListItem
                {
                    Text = teacher.FirstName + " " + teacher.LastName,
                    Value = teacher.UserId,
                });
            }

            Teachers = teachers;
            List<SelectListItem> gradeLevels = new List<SelectListItem>();
            gradeLevels.Add(new SelectListItem
            {
                Text = "N/A",
                Value =
                    ""
            });
            gradeLevels.Add(new SelectListItem
            {
                Text = "Kindergarten",
                Value = "0"
            });
            for (int i = 1; i <= 8; i++)
                gradeLevels.Add(new SelectListItem
                {
                    Text =
                        i.ToString(),
                    Value = i.ToString()
                });
            gradeLevels.Add(new SelectListItem
            {
                Text = "Freshman",
                Value = "9"
            });
            gradeLevels.Add(new SelectListItem
            {
                Text = "Sophomore",
                Value = "10"
            });
            gradeLevels.Add(new SelectListItem
            {
                Text = "Junior",
                Value
                    = "11"
            });
            gradeLevels.Add(new SelectListItem
            {
                Text = "Senior",
                Value
                    = "12"
            });

            DropdownGradeLevels = gradeLevels;
        }
    }
}

