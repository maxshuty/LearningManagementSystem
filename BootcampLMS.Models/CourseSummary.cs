using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using BootcampLMS.Models;

namespace BootcampLMS.Models
{
    public class CourseSummary
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public bool IsArchived { get; set; }

        public AnalyticsModel MyAnalyticsModel { get; set; }
    }


}
