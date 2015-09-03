using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampLMS.UI.Models
{
    public class GradebookViewModel
    {
        public List<List<AssignmentViewModel>> MyAssignmentViewModels { get; set; }

        public GradebookViewModel()
        {
            MyAssignmentViewModels = new List<List<AssignmentViewModel>>();
        }
    }
}