using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampLMS.UI.Models
{
    public class RosterTableViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RosterId { get; set; }
        public int CourseId { get; set; }
    }
}